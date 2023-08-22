#nullable enable
using System;

namespace NWDFoundation.Logger
{
    /// <summary>
    /// Interface allowing to create classes which will be used as debugger in particular environments.
    /// </summary>
    public interface INWDLogger
    {
        /// <summary>
        /// Method to write message in console
        /// </summary>
        /// <param name="sLogLevel"></param>
        /// <param name="sString"></param>
        /// <param name="sObject"></param>
        public void WriteLog(NWDLogLevel sLogLevel, NWDLogCategory sLogCategory, string sString, object? sObject);
        /// <summary>
        /// Method to write message in console
        /// </summary>
        /// <param name="sLogLevel"></param>
        /// <param name="sTitle"></param>
        /// <param name="sObject"></param>
        /// <param name="sMessages"></param>
        public void WriteLog(NWDLogLevel sLogLevel, NWDLogCategory sLogCategory, string sTitle, object? sObject, string[] sMessages);
        /// <summary>
        /// Return if debugger is active or not
        /// </summary>
        /// <returns></returns>
        public bool IsActivated();
        /// <summary>
        /// The <see cref="NWDLogLevel"/> to be displayed by the logger.
        /// </summary>
        /// <returns></returns>
        public NWDLogLevel LogLevel();
        /// <summary>
        /// Sets the <see cref="NWDLogLevel"/> to be displayed by the logger.
        /// </summary>
        /// <returns></returns>
        public void SetLogLevel(NWDLogLevel sLogLevel);
        /// <summary>
        /// The default <see cref="NWDLogLevel"/> to be displayed by the logger.
        /// </summary>
        /// <returns></returns>
        public NWDLogLevel DefaultLogLevel();
    }
}
#nullable disable