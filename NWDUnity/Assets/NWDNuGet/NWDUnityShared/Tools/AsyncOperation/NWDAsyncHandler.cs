using System;

namespace NWDUnityShared.Tools
{
    public struct NWDAsyncHandler
    {
        public NWDAsyncOperation Operation { get; private set; }

        /// <summary>
        /// <inheritdoc cref="NWDAsyncOperation.Progress"/>.<br/>
        /// The value '1' is automatically set at the end of the <see cref="NWDAsyncOperation"/> when successful.
        /// </summary>
        public float Progress
        {
            get
            {
                return Operation.Progress;
            }
            set
            {
                if (value >= 0 && value < 1)
                {
                    Operation.Progress = value;
                }
            }
        }

        /// <summary>
        /// <inheritdoc cref="NWDAsyncOperation.AddWarning(Exception)"/>
        /// </summary>
        /// <param name="warning"><inheritdoc cref="NWDAsyncOperation.AddWarning(Exception)"/></param>
        internal void AddWarning(Exception warning = null)
        {
            Operation.AddWarning(warning);
        }

        public NWDAsyncHandler (NWDAsyncOperation sOperation)
        {
            Operation = sOperation;
        }
    }
}
