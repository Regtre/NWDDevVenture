using System;
using Newtonsoft.Json;
using NWDFoundation.Exchanges;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDDatabaseBasicModel // Cannot be abstract as must be unserialized for Unity propagation
    {
        public Int64 Creation { set; get; } //  -9223372036854775808 to 9223372036854775807
        public Int64 Modification { set; get; } //  -9223372036854775808 to 9223372036854775807
        public bool Active { set; get; } = true;
        public bool Trashed { set; get; } = false;
        public ulong Reference { set; get; } //  0 to 18 446 744 073 709 551 615
        [JsonIgnore]
        public ulong RowId { set; get; } //  0 to 18 446 744 073 709 551 615

        public NWDDatabaseBasicModel() { }

        public NWDDatabaseBasicModel(NWDDatabaseBasicModel sOther)
        {
            Creation = sOther.Creation;
            Modification = sOther.Modification;
            Active = sOther.Active;
            Trashed = sOther.Trashed;
            Reference = sOther.Reference;
            RowId = sOther.RowId;
        }
        
        
        public virtual void CopyFrom(NWDDatabaseBasicModel sOther)
        {
            Console.WriteLine("CopyFrom NWDDatabaseBasicModel");
            Creation = sOther.Creation;
            Modification = sOther.Modification;
            Active = sOther.Active;
            Trashed = sOther.Trashed;
            Reference = sOther.Reference;
            RowId = sOther.RowId;
        }
    }
}