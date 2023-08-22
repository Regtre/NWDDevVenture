using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NWDUnityShared.Tools
{
    public class NWDGUIAsyncOperation : CustomYieldInstruction, INWDAsyncOperation
    {
        protected const EventType eventType = EventType.Layout;

        protected NWDAsyncOperation Operation;

        /// <summary>
        /// <inheritdoc cref="onDone"/><br/>
        /// This call is thread safe.
        /// </summary>
        public Action<NWDAsyncOperation> OnDone
        {
            get
            {
                UpdateData();
                return Operation.OnDone;
            }
            set
            {
                UpdateData();
                Operation.OnDone = value;
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
                UpdateData();
                return progress;
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
                UpdateData();
                return state;
            }
        }

        /// <summary>
        /// The list of non blocking errors that happened during <see cref="NWDAsyncOperation"/> execution.
        /// </summary>
        private List<Exception> warnings = null;

        /// <summary>
        /// <inheritdoc cref="warnings"/><br/>
        /// This call is thread safe.
        /// </summary>
        public List<Exception> Warnings
        {
            get
            {
                UpdateData();
                return warnings;
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
                UpdateData();
                return error;
            }
        }

        private bool isDone = false;

        /// <summary>
        /// Is the <see cref="System.Threading.Tasks.Task"/> finished ?<br/>
        /// This call is thread safe.
        /// </summary>
        public bool IsDone
        {
            get
            {
                UpdateData();
                return isDone;
            }
        }

        /// <summary>
        /// Make it so the system works with coroutines.
        /// </summary>
        public override bool keepWaiting => !IsDone;

        /// <summary>
        /// The <see cref="Task"/> <see cref="CancellationToken"/> of the operation.
        /// </summary>
        public CancellationToken Token
        {
            get
            {
                UpdateData();
                return Operation.Token;
            }
        }

        public NWDGUIAsyncOperation(NWDAsyncOperation sOperation)
        {
            Operation = sOperation;
            Operation.OnDone += (_) => Internal_UpdateData();
        }

        /// <summary>
        /// Cancels the <see cref="NWDAsyncOperation"/>.
        /// </summary>
        public void Cancel ()
        {
            UpdateData();
            Operation.Cancel ();
        }

        /// <summary>
        /// Waits for the async operation to be done asynchronously.
        /// </summary>
        /// <returns></returns>
        public Task Wait()
        {
            UpdateData();
            return Operation.Wait();
        }

        /// <summary>
        /// Updates cached data at the right moment to prevent GUILayout error.
        /// </summary>
        protected void UpdateData ()
        {
            if (Event.current != null && Event.current.type == eventType)
            {
                Internal_UpdateData();
            }
        }

        /// <summary>
        /// Updates cached data at the right moment to prevent GUILayout error.
        /// </summary>
        protected virtual void Internal_UpdateData()
        {
            progress = Operation.Progress;
            state = Operation.State;
            warnings = Operation.Warnings;
            error = Operation.Error;
            isDone = Operation.IsDone;
        }

        static public implicit operator NWDGUIAsyncOperation(NWDAsyncOperation sOperation)
        {
            return new NWDGUIAsyncOperation (sOperation);
        }
    }
    public class NWDGUIAsyncOperation<T> : NWDGUIAsyncOperation, INWDAsyncOperation<T>
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
                UpdateData();
                return result;
            }
        }

        public NWDGUIAsyncOperation(NWDAsyncOperation<T> sOperation) : base(sOperation)
        {

        }

        /// <summary>
        /// <inheritdoc cref="NWDGUIAsyncOperation.UpdateData()"/><br/>
        /// </summary>
        protected override void Internal_UpdateData()
        {
            base.UpdateData();
            NWDAsyncOperation<T> tOperation = Operation as NWDAsyncOperation<T>;
            result = tOperation.Result;
        }

        static public implicit operator NWDGUIAsyncOperation<T>(NWDAsyncOperation<T> sOperation)
        {
            return new NWDGUIAsyncOperation<T>(sOperation);
        }
    }
}
