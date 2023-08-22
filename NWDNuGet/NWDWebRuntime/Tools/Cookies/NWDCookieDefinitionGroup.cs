namespace NWDWebRuntime.Tools.Cookies
{
    public enum NWDCookieDefinitionGroup
    {
        Functional, // never delete by consent dependency (force to if deletable = false)
        Consent, // need to use cookies ...  deletable =true
        Advertisement, // DELETABLE if deletable == true
        Analytics, // DELETABLE if deletable == true
        Optional, // DELETABLE if deletable == true
        Design, // DELETABLE if deletable == true
        Preference, // DELETABLE if deletable == true
    }
}