using System;

namespace NWDUnityEditor.UserSettings
{
    [Serializable]
    public class NWDRoleUserSettings
    {
        static public readonly string DefaultNickname = Environment.UserName + " <" + Environment.MachineName + ">";

        public string RoleName = string.Empty;

        public string PublicToken = string.Empty;
        public string PrivateToken = string.Empty;

        public string Nickname = DefaultNickname;

        public bool CanEditMetaData = false;
        public bool CanCreateMetaData = false;
    }
}
