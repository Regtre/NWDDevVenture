using System;
using NWDFoundation.Models;

namespace NWDFoundation.Models
{
    public class NWDRelationship : NWDBasicModel
    {
       public NWDReference<NWDAccount> AccountA { get; set; }
       public NWDReference<NWDAccount> AccountB { get; set; }
       public NWDRelationshipState RelationshipState { get; set; }
       public int ModificationDate { get; set; }
       public string Code { get; set; }
       public int CodeExpiryDate { get; set; }
       
     
       public override bool Equals(object sObj)
       {
           return sObj!= null && Reference == ((NWDRelationship)sObj).Reference;
       }
       public override int GetHashCode()
       {
           return 1;
       }
    }
    
}