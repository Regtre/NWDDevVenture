namespace NWDFoundation.Models
{

    public enum NWDRelationshipState
    {
        Pending = 0,
        WaitingConfirmation = 1,
        Accepted = 2,
        Refused = 3,
        Deleted = 4,
        Timeout = 5,
        None = 6,
    }
}
