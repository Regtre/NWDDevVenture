using NWDUnityShared.Engine;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NWDUnityShared.Tools
{
    public class NWDAsyncOperation : CustomYieldInstruction, INWDAsyncOperation
    {
        protected object _lock = new object();

        /// <summary>
        /// The <see cref="CancellationTokenSource"/> of the <see cref="NWDAsyncOperation"/>.
        /// </summary>
        private CancellationTokenSource CancellationSource = new CancellationTokenSource();

        /// <summary>
        /// The current <see cref="System.Threading.Tasks.Task"/> of the <see cref="NWDAsyncOperation"/>.
        /// </summary>
        public Task task;

        /// <summary>
        /// <inheritdoc cref="task"/><br/>
        /// This call is thread safe.
        /// </summary>
        internal Task Task
        {
            set
            {
                lock (_lock)
                {
                    task = value;
                }
            }
        }

        /// <summary>
        /// Callback triggered when the <see cref="NWDAsyncOperation"/> is finihsed.
        /// </summary>
        private Action<NWDAsyncOperation> onDone;

        /// <summary>
        /// <inheritdoc cref="onDone"/><br/>
        /// This call is thread safe.
        /// </summary>
        public Action<NWDAsyncOperation> OnDone
        {
            get
            {
                lock (_lock)
                {
                    return onDone;
                }
            }
            set
            {
                lock (_lock)
                {
                    onDone = value;
                }
            }
        }

        /// <summary>
        /// The progress of the <see cref="System.Threading.Tasks.Task"/>.<br/>
        /// The value is bteween 0 (task started) and 1 (task is finished) included.
        /// </summary>
        private float progress = 0;

        /// <summary>
        /// <inheritdoc cref="progress"/><br/>
        /// This call is thread safe.
        /// </summary>
        public float Progress
        {
            get
            {
                lock (_lock)
                {
                    return progress;
                }
            }
            internal set
            {
                lock (_lock)
                {
                    progress = value;
                }
            }
        }

        /// <summary>
        /// The state of the <see cref="NWDAsyncOperation"/>.
        /// </summary>
        private NWDAsyncOperationState state;

        /// <summary>
        /// <inheritdoc cref="state"/><br/>
        /// This call is thread safe.
        /// </summary>
        public NWDAsyncOperationState State
        {
            get
            {
                lock (_lock)
                {
                    if (state == NWDAsyncOperationState.Success && warnings.Count > 0)
                    {
                        return NWDAsyncOperationState.Warning;
                    }

                    return state;
                }
            }
            internal set
            {
                lock (_lock)
                {
                    // Prevents the state to go from "Failure" to a "Success".
                    if (value < NWDAsyncOperationState.Success || state < value)
                    {
                        state = value;
                    }

                    if (state >= NWDAsyncOperationState.Success)
                    {
                        NWDUnityEngine.Instance.ThreadManager.CallOnMainThread(CallOnDone);
                    }
                }
            }
        }

        /// <summary>
        /// The list of non blocking errors that happened during <see cref="NWDAsyncOperation"/> execution.
        /// </summary>
        private List<Exception> warnings = new List<Exception>();

        /// <summary>
        /// <inheritdoc cref="warnings"/><br/>
        /// This call is thread safe.
        /// </summary>
        public List<Exception> Warnings
        {
            get
            {
                lock (_lock)
                {
                    return warnings;
                }
            }
        }

        /// <summary>
        /// The error that caused a <see cref="NWDAsyncOperationState.Failure"/>.
        /// </summary>
        private Exception error;

        /// <summary>
        /// <inheritdoc cref="error"/><br/>
        /// This call is thread safe.
        /// </summary>
        public Exception Error
        {
            get
            {
                lock(_lock)
                {
                    return error;
                }
            }
            internal set
            {
                lock (_lock)
                {
                    error = value;
                }
            }
        }

        /// <summary>
        /// Is the <see cref="System.Threading.Tasks.Task"/> finished ?<br/>
        /// This call is thread safe.
        /// </summary>
        public bool IsDone
        {
            get
            {
                return State >= NWDAsyncOperationState.Success;
            }
        }

        /// <summary>
        /// Make it so the system works with coroutines.
        /// </summary>
        public override bool keepWaiting => !IsDone;

        /// <summary>
        /// The <see cref="System.Threading.Tasks.Task"/> <see cref="CancellationToken"/> of the operation.
        /// </summary>
        public CancellationToken Token
        {
            get
            {
                lock (_lock)
                {
                    return CancellationSource.Token;
                }
            }
        }

        /// <summary>
        /// Add a warning to the warning list and set the <see cref="State"/> to <see cref="NWDAsyncOperationState.Warning"/>.
        /// </summary>
        /// <param name="warning">The warning raised.</param>
        internal void AddWarning (Exception warning = null)
        {
            lock (_lock)
            {
                if (warning != null)
                {
                    warnings.Add(warning);
                }
            }
        }

        /// <summary>
        /// Called once the <see cref="NWDAsyncOperation"/> is finished.
        /// </summary>
        private void CallOnDone ()
        {
            lock (_lock)
            {
                onDone?.Invoke(this);
            }
        }

        /// <summary>
        /// Cancels the <see cref="NWDAsyncOperation"/>.
        /// </summary>
        public void Cancel ()
        {
            lock (_lock)
            {
                CancellationSource.Cancel();
            }
        }

        /// <summary>
        /// Waits for the async operation to be done asynchronously.
        /// </summary>
        /// <returns></returns>
        public Task Wait()
        {
            return Task.Run(async () =>
            {
                while (!IsDone)
                {
                    await Task.Yield();
                }
            });
        }
    }
    public class NWDAsyncOperation<T> : NWDAsyncOperation, INWDAsyncOperation<T>
    {
        /// <summary>
        /// The result data of the <see cref="NWDAsyncOperation{T}"/>.
        /// </summary>
        private T result;

        /// <summary>
        /// <inheritdoc cref="result"/><br/>
        /// This call is thread safe.
        /// </summary>
        public T Result
        {
            get
            {
                lock (_lock)
                {
                    return result;
                }
            }
            internal set
            {
                lock (_lock)
                {
                    result = value;
                }
            }
        }
    }
}
