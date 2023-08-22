using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NWDUnityShared.Tools
{
    public class NWDAsyncOperationFactory
    {
        static public NWDAsyncOperation NewTask<TS>(TS sScheduler, Action<NWDAsyncHandler, CancellationToken> sRunner) where TS : NWDTaskScheduler<TS>, new()
        {
            return NewTask(sScheduler, (sHandler, sCancellation) => sScheduler.Factory.StartNew(() => sRunner?.Invoke(sHandler, sCancellation), sCancellation));
        }
        static public NWDAsyncOperation NewTask<TS> (TS sScheduler, Func<NWDAsyncHandler, CancellationToken, Task> sRunner) where TS : NWDTaskScheduler<TS>, new ()
        {
            NWDAsyncOperation tOperation = new NWDAsyncOperation ();
            tOperation.State = NWDAsyncOperationState.Pending;
            tOperation.Task = GenerateTask(tOperation, sScheduler, sRunner);

            return tOperation;
        }

        static public NWDAsyncOperation<T> NewTask<TS, T>(TS sScheduler, Func<NWDAsyncHandler, CancellationToken, T> sRunner) where TS : NWDTaskScheduler<TS>, new()
        {
            return NewTask(sScheduler, (sHandler, sCancellation) => sScheduler.Factory.StartNew<T>(() => sRunner.Invoke(sHandler, sCancellation), sCancellation));
        }
        static public NWDAsyncOperation<T> NewTask<TS, T>(TS sScheduler, Func<NWDAsyncHandler, CancellationToken, Task<T>> sRunner) where TS : NWDTaskScheduler<TS>, new()
        {
            NWDAsyncOperation<T> tOperation = new NWDAsyncOperation<T>();
            tOperation.State = NWDAsyncOperationState.Pending;
            tOperation.Task = GenerateTask(tOperation, sScheduler, sRunner);

            return tOperation;
        }

        static public NWDAsyncOperation NewReccurentTask<TS>(TS sScheduler, Action<NWDAsyncHandler, CancellationToken> sRunner, int sPeriode) where TS : NWDTaskScheduler<TS>, new()
        {
            return NewReccurentTask(sScheduler, (sHandler, sCancellation) => sScheduler.Factory.StartNew(() => sRunner?.Invoke(sHandler, sCancellation), sCancellation), sPeriode);
        }
        static public NWDAsyncOperation NewReccurentTask<TS>(TS sScheduler, Func<NWDAsyncHandler, CancellationToken, Task> sRunner, int sPeriode) where TS : NWDTaskScheduler<TS>, new()
        {
            NWDAsyncOperation tOperation = null;
            if (sPeriode > 0)
            {
                tOperation = new NWDAsyncOperation();
                tOperation.State = NWDAsyncOperationState.Pending;
                ReccurentTaskRunner(tOperation, sScheduler, sRunner, sPeriode);
            }

            return tOperation;
        }

        static public NWDAsyncOperation NewReccurentTask<TS, T>(TS sScheduler, Func<NWDAsyncHandler, CancellationToken, T> sRunner, int sPeriode) where TS : NWDTaskScheduler<TS>, new()
        {
            return NewReccurentTask(sScheduler, (sHandler, sCancellation) => sScheduler.Factory.StartNew<T>(() => sRunner.Invoke(sHandler, sCancellation), sCancellation), sPeriode);
        }
        static public NWDAsyncOperation<T> NewReccurentTask<TS, T>(TS sScheduler, Func<NWDAsyncHandler, CancellationToken, Task<T>> sRunner, int sPeriode) where TS : NWDTaskScheduler<TS>, new()
        {
            NWDAsyncOperation<T> tOperation = null;
            if (sPeriode > 0)
            {
                tOperation = new NWDAsyncOperation<T>();
                tOperation.State = NWDAsyncOperationState.Pending;
                ReccurentTaskRunner(tOperation, sScheduler, sRunner, sPeriode);
            }

            return tOperation;
        }

        static private void ReccurentTaskRunner<TS>(NWDAsyncOperation sOperation, TS sScheduler, Func<NWDAsyncHandler, CancellationToken, Task> sRunner, int sPeriode) where TS : NWDTaskScheduler<TS>, new()
        {
            if (sScheduler.IsProcessingReccurentTasks)
            {
                sOperation.Task = GenerateTask(sOperation, sScheduler, sRunner);

                Task.Run(async () =>
                {
                    Task tDelay = Task.Delay(sPeriode * 1000, sOperation.Token);
                    await Task.WhenAll(sOperation.Wait(), tDelay);
                    ReccurentTaskRunner(sOperation, sScheduler, sRunner, sPeriode);
                }, sOperation.Token);
            }
            else
            {
                sOperation.State = NWDAsyncOperationState.Stopped;
            }
        }

        static private void ReccurentTaskRunner<TS, T>(NWDAsyncOperation<T> sOperation, TS sScheduler, Func<NWDAsyncHandler, CancellationToken, Task<T>> sRunner, int sPeriode) where TS : NWDTaskScheduler<TS>, new()
        {
            if (sScheduler.IsProcessingReccurentTasks)
            {
                sOperation.Task = GenerateTask(sOperation, sScheduler, sRunner);

                Task.Run(async () =>
                {
                    Task tDelay = Task.Delay(sPeriode * 1000, sOperation.Token);
                    await Task.WhenAll(sOperation.Wait(), tDelay);
                    ReccurentTaskRunner(sOperation, sScheduler, sRunner, sPeriode);

                }, sOperation.Token);
            }
            else
            {
                sOperation.State = NWDAsyncOperationState.Stopped;
            } 
        }

        static private Task GenerateTask<TS> (NWDAsyncOperation sOperation, TS sScheduler, Func<NWDAsyncHandler, CancellationToken, Task> sRunner) where TS : NWDTaskScheduler<TS>, new()
        {
            return sScheduler.Factory.StartNew(async () =>
            {
                try
                {
                    sOperation.State = NWDAsyncOperationState.Running;
                    sOperation.Progress = 0;
                    await sRunner.Invoke(new NWDAsyncHandler(sOperation), sOperation.Token);
                    if (sOperation.Token.IsCancellationRequested)
                    {
                        throw new TaskCanceledException();
                    }
                    sOperation.State = NWDAsyncOperationState.Success;
                    sOperation.Progress = 1;
                    if (sOperation.Token.IsCancellationRequested)
                    {
                        throw new TaskCanceledException();
                    }
                }
                catch (TaskCanceledException e)
                {
                    sOperation.State = NWDAsyncOperationState.Canceled;
                    Debug.LogWarning(e);
                    return;
                }
                catch (Exception e)
                {
                    sOperation.Error = e;
                    sOperation.State = NWDAsyncOperationState.Failure;
                    Debug.LogException(e);
                }
            }, sOperation.Token);
        }

        static private Task GenerateTask<TS, T>(NWDAsyncOperation<T> sOperation, TS sScheduler, Func<NWDAsyncHandler, CancellationToken, Task<T>> sRunner) where TS : NWDTaskScheduler<TS>, new()
        {
            return sScheduler.Factory.StartNew(async () =>
            {
                try
                {
                    sOperation.State = NWDAsyncOperationState.Running;
                    sOperation.Progress = 0;
                    sOperation.Result = await sRunner.Invoke(new NWDAsyncHandler(sOperation), sOperation.Token);
                    if (sOperation.Token.IsCancellationRequested)
                    {
                        throw new TaskCanceledException();
                    }
                    sOperation.State = NWDAsyncOperationState.Success;
                    sOperation.Progress = 1;
                    if (sOperation.Token.IsCancellationRequested)
                    {
                        throw new TaskCanceledException();
                    }
                }
                catch (TaskCanceledException)
                {
                    sOperation.State = NWDAsyncOperationState.Canceled;
                    return;
                }
                catch (Exception e)
                {
                    sOperation.Error = e;
                    sOperation.State = NWDAsyncOperationState.Failure;
                    Debug.LogException(e);
                }
            }, sOperation.Token);
        }
    }
}
