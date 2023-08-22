using System;

namespace NWDFoundation.Models.Enums
{
    [Serializable]
    public enum NWDAccountSignType : int
    {
        /// <summary>
        /// Unknown kind
        /// </summary>
        None = 0, // NEVER CHANGE INT VALUE !
        /// <summary>
        /// Use network card UniqueID to create SignHash
        /// </summary>
        DeviceId = 1, // NEVER CHANGE INT VALUE !
        /// <summary>
        /// Use email and password to create SignHash
        /// </summary>
        EmailPassword = 10, // NEVER CHANGE INT VALUE !
        /// <summary>
        /// Use email and password to create SignHash
        /// </summary>
        [Obsolete("Use to migration from Net-Worked-Data 2 or Dicolatin service")]
        OldEmailPassword = 75, // NEVER CHANGE INT VALUE !
        /// <summary>
        /// Use login, password and email to create SignHash
        /// </summary>
        LoginEmailPassword = 11, // NEVER CHANGE INT VALUE !
        /// <summary>
        /// Use login and password to create SignHash
        /// </summary>
        LoginPassword = 12, // NEVER CHANGE INT VALUE !
        /// <summary>
        /// Use FacebookID to create SignHash
        /// </summary>
        Facebook = 20, // NEVER CHANGE INT VALUE !
        /// <summary>
        /// Use GoogleID to create SignHash
        /// </summary>
        Google = 21, // NEVER CHANGE INT VALUE !
        /// <summary>
        /// Use AppleID to create SignHash
        /// </summary>
        Apple = 22, // NEVER CHANGE INT VALUE !
        /// <summary>
        /// Use MicrosoftID to create SignHash
        /// </summary>
        Microsoft = 23, // NEVER CHANGE INT VALUE !
        /// <summary>
        /// Use TwitterID to create SignHash
        /// </summary>
        Twitter = 24, // NEVER CHANGE INT VALUE !
        /// <summary>
        /// Use LinkedInID to create SignHash
        /// </summary>
        LinkedIn = 25, // NEVER CHANGE INT VALUE !
        /// <summary>
        /// Use DiscordID to create SignHash
        /// </summary>
        Discord = 30, // NEVER CHANGE INT VALUE !
    }
}