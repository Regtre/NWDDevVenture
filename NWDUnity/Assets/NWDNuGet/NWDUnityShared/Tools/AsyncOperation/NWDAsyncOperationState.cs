namespace NWDUnityShared.Tools
{
    /// <summary>
    /// The state of the <see cref="NWDAsyncOperation"/>.
    /// </summary>
    public enum NWDAsyncOperationState : byte
    {
        /// <summary>
        /// The <see cref="NWDAsyncOperation"/> is waiting to be launched.
        /// </summary>
        Pending = 0,
        /// <summary>
        /// The <see cref="NWDAsyncOperation"/> is started and running.
        /// </summary>
        Running = 1,
        /// <summary>
        /// The <see cref="NWDAsyncOperation"/> finished successfuly.
        /// </summary>
        Success = 2,
        /// <summary>
        /// The <see cref="NWDAsyncOperation"/> reached the end but something unexpected happened.
        /// </summary>
        Warning = 3,
        /// <summary>
        /// The <see cref="NWDAsyncOperation"/> failed.
        /// </summary>
        Failure = 4,
        /// <summary>
        /// The <see cref="NWDAsyncOperation"/> was canceled.
        /// </summary>
        Canceled = 5,
        /// <summary>
        /// The <see cref="NWDAsyncOperation"/> was stopped by the scheduler.<br/>
        /// The result of the operation cannot be used.
        /// </summary>
        Stopped = 6,
    }
}
