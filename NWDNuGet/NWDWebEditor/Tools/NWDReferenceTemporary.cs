using NWDFoundation.Tools;

namespace NWDWebEditor.Tools;

public class NWDReferenceTemporary
{
    static private List<long> _ListOfTemporary = new List<long>();
    static public long GetTemporaryReference()
    {
        long rReturn = -(long)NWDRandom.UnsignedIntNumeric(6);
        while (_ListOfTemporary.Contains(rReturn))
        {
            rReturn = -(long)NWDRandom.UnsignedIntNumeric(6);
        }
        _ListOfTemporary.Add(rReturn);
        return rReturn;
    }

    static public void FreeTemporaryReference(long sReference)
    {
        _ListOfTemporary.Remove(sReference);
    }
}