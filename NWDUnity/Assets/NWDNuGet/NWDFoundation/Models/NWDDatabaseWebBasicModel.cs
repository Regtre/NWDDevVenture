using System;

namespace NWDFoundation.Models
{
    [Serializable]
    public abstract class NWDDatabaseWebBasicModel : NWDDatabaseBasicModel
    {
        #region properties

        public ulong ProjectId { set; get; }

        #endregion

        #region constructors

        protected NWDDatabaseWebBasicModel() { }

        protected NWDDatabaseWebBasicModel(NWDDatabaseWebBasicModel sBasicModel) : base(sBasicModel)
        {
            RowId = sBasicModel.RowId;
        }

        #endregion
    }
}