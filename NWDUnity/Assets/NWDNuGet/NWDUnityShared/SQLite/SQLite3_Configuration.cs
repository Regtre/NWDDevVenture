namespace NWDUnityShared.SQLite
{
    public static partial class SQLite3
    {
#if UNITY_EDITOR_OSX
        private const string DLL_NAME = "libsqlite3.0";
#elif UNITY_EDITOR_WIN
        private const string DLL_NAME = "sqlite3";
#elif UNITY_EDITOR_LINUX
        private const string DLL_NAME = "sqlite3";
#elif UNITY_STANDALONE_OSX
        private const string DLL_NAME = "libsqlite3.0";
#elif UNITY_STANDALONE_WIN
        private const string DLL_NAME = "sqlite3";
#elif UNITY_STANDALONE_LINUX
        private const string DLL_NAME = "sqlite3";
#elif UNITY_ANDROID
		private const string DLL_NAME = "sqlite3";
#elif UNITY_IOS
		private const string DLL_NAME = "__Internal";
#elif UNITY_TVOS
		private const string DLL_NAME = "__Internal";
#endif
    }
}