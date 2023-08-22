using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NWDUnityShared.Tools
{
    /// <summary>
    /// Implement this <see cref="TaskScheduler"/> to perform NetWorkedData async tasks.<br/>
    /// <br/>
    /// Note: This scheduler is made to process <see cref="Task"/> using the FIFO method.
    /// Consequently any <see cref="Task"/> run by the scheduler won't be executed if another is being processed.<br/>
    /// It is not necessary run onto several threads though.
    /// </summary>
    public abstract class NWDTaskScheduler<T> : TaskScheduler where T : NWDTaskScheduler<T>
    {
        [ThreadStatic]
        static private bool IsProcessingTask = false;

        private TaskFactory factory = null;

        public TaskFactory Factory => factory;

        public NWDTaskScheduler()
        {
            factory = new TaskFactory(this);
        }

        private object _lock = new object();

        private Queue<Task> Tasks = new Queue<Task>();

        public abstract bool IsProcessingReccurentTasks { get; }

        /// <summary>
        /// Get an <see cref="IEnumerable{Task}"/> of the queued <see cref="Task"/>.
        /// </summary>
        /// <returns>The <see cref="IEnumerable{Task}"/>.</returns>
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            lock (_lock)
            {
                return Tasks;
            }
        }

        /// <summary>
        /// Queue a <see cref="Task"/> and processes it if necessary.
        /// </summary>
        /// <param name="sTask">The <see cref="Task"/> to enqueue.</param>
        protected override void QueueTask(Task sTask)
        {
            lock (_lock)
            {
                Tasks.Enqueue(sTask);
                if (!IsProcessingTask)
                {
                    ProcessTasks();
                }
            }
        }

        /// <summary>
        /// Process the queue.
        /// </summary>
        private void ProcessTasks ()
        {
            ThreadPool.UnsafeQueueUserWorkItem(_ =>
            {
                IsProcessingTask = true;
                try
                {
                    while (true)
                    {
                        lock (_lock)
                        {
                            if (Tasks.Count == 0)
                            {
                                break;
                            }
                            Task tTask = Tasks.Dequeue();
                            base.TryExecuteTask(tTask);
                        }
                    }
                }
                finally
                {
                    IsProcessingTask = false;
                }
            }, null);
        }

        /// <summary>
        /// Attemps to execute a new <see cref="Task"/> with the <see cref="TaskScheduler"/>.
        /// </summary>
        /// <param name="sTask">The <see cref="Task"/> to execute.</param>
        /// <param name="sTaskWasPreviouslyQueued">Whether the <see cref="Task"/> was in queue or not.</param>
        /// <returns><i>true</i> if the <see cref="Task"/> can be executed, <i>false</i> otherwise.</returns>
        protected override bool TryExecuteTaskInline(Task sTask, bool sTaskWasPreviouslyQueued)
        {
            bool rResult = false;
            if (!IsProcessingTask && sTaskWasPreviouslyQueued)
            {
                rResult = TryDequeue(sTask);
            }
            return rResult;
        }

        /// <summary>
        /// Tries to dequeue the <see cref="Task"/> from the <see cref="TaskScheduler"/>.<br/>
        /// Only works if the <see cref="Task"/> is the next that must be processed.
        /// </summary>
        /// <param name="sTask">The <see cref="Task"/> to dequeue.</param>
        /// <returns><i>true</i> if the <see cref="Task"/> was dequeued, <i>false</i> otherwise.</returns>
        protected override bool TryDequeue(Task sTask)
        {
            bool rResult = false;
            lock (_lock)
            {
                if (sTask == Tasks.Peek())
                {
                    Tasks.Dequeue();
                    rResult = true;
                }
            }
            return rResult;
        }
    }
}
