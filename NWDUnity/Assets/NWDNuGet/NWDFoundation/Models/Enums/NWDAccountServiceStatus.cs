namespace NWDFoundation.Models.Enums
{
    public enum NWDAccountServiceStatus
    {
        IsInactive = 0,
        
        IsActive = 1,
        
        //waiting activation and then add duration to now
        IsWaitingActivation = 3, 
        
        // Alert user ... 
        IsBan = 9,
        
        // copy this service as subservice and stay as subservice ( to reatribue later)
        IsSubServiced = 88,
        
        // copy as original and desactive this service 
        IsToTranfert= 99,
            
    }
}