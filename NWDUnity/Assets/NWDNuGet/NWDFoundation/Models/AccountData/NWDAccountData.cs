using System;

namespace NWDFoundation.Models
{
    /// <summary>
    /// Data directly recorded in DataBas : no serialization storage.
    /// </summary>
    [Serializable]
    public abstract class NWDAccountData : NWDBasicModel, INWDAccountDependence
    {
        /// <summary>
        /// Reference to Account
        /// </summary>
        public ulong Account { set; get; }

        /// <summary>
        /// Range of database to use for this Account, represent the database to use (by DAO settings)
        /// </summary>
        public ushort Range { set; get; }

    }
}