using System;

namespace NWDFoundation.Models.Enums
{
    [Serializable]
    public enum NWDAccountSignAction
    {
        None = 0, // NEVER CHANGE INT VALUE !!!
        TryToAssociate = 10, // NEVER CHANGE INT VALUE !!!
        Associated = 11, // NEVER CHANGE INT VALUE !!!
        ErrorAssociated = 12, // NEVER CHANGE INT VALUE !!!
        TryToDissociate = 20, // NEVER CHANGE INT VALUE !!!
        Dissociated = 21, // NEVER CHANGE INT VALUE !!!
        //ErrorDissociated = 22, // no possible case  // NEVER CHANGE INT VALUE !!!
    }
}