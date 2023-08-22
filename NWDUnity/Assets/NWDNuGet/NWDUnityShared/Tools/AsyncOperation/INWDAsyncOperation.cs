using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NWDUnityShared.Tools
{
    public interface INWDAsyncOperation
    {
        /// <summary>
        /// <inheritdoc cref="onDone"/><br/>
        /// This call is thread safe.
        /// </summary>
        public Action<NWDAsyncOperation> OnDone { get; }

        /// <summary>
        /// <inheritdoc cref="progress"/><br/>
        /// This call is thread safe.
        /// </summary>
        public float Progress { get; }

        /// <summary>
        /// <inheritdoc cref="state"/><br/>
        /// This call is thread safe.
        /// </summary>
        public NWDAsyncOperationState State { get; }

        /// <summary>
        /// <inheritdoc cref="warnings"/><br/>
        /// This call is thread safe.
        /// </summary>
        public List<Exception> Warnings { get; }

        /// <summary>
        /// <inheritdoc cref="error"/><br/>
        /// This call is thread safe.
        /// </summary>
        public Exception Error { get; }

        /// <summary>
        /// Is the <see cref="Task"/> finished ?<br/>
        /// This call is thread safe.
        /// </summary>
        public bool IsDone { get; }

        /// <summary>
        /// The <see cref="Task"/> <see cref="CancellationToken"/> of the operation.
        /// </summary>
        public CancellationToken Token { get; }

        /// <summary>
        /// Cancels the <see cref="NWDAsyncOperation"/>.
        /// </summary>
        public void Cancel();

        /// <summary>
        /// Waits for the async operation to be done asynchronously.
        /// </summary>
        /// <returns></returns>
        public Task Wait();
    }

    public interface INWDAsyncOperation<T> : INWDAsyncOperation
    {
        /// <summary>
        /// <inheritdoc cref="result"/><br/>
        /// This call is thread safe.
        /// </summary>
        public T Result { get; }
    }
}
