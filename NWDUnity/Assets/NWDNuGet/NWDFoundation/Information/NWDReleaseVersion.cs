using NWDFoundation.Tools;

namespace NWDFoundation.Information
{
    public class NWDReleaseVersion
    {
        public string Namespace = string.Empty;
        public string Path = string.Empty;
        private int Major = 0;
        private int Minor = 0;
        private int Built = 0;

        public NWDReleaseVersion(string sNamespace, string sPath, int sMajor, int sMinor, int sBuilt)
        {
            Namespace = sNamespace;
            Path = sPath;
            Major = sMajor;
            Minor = sMinor;
            Built = sBuilt;
        }

        public NWDReleaseVersion( int sMajor, int sMinor, int sBuilt)
        {
            Major = sMajor;
            Minor = sMinor;
            Built = sBuilt;
        }

        public static NWDReleaseVersion Version(string sNamespace, string sPath, int sMajor, int sMinor, int sBuilt)
        {
            return new NWDReleaseVersion(sNamespace, sPath, sMajor, sMinor, sBuilt);
        }

        public static NWDReleaseVersion Version(int sMajor, int sMinor, int sBuilt)
        {
            return new NWDReleaseVersion(sMajor, sMinor, sBuilt);
        }

        public void Increment(bool sMajor = false, bool sMinor = false, bool sBuilt = true)
        {
            if (sMajor == true)
            {
                Major++;
                Minor = 0;
                Built = 0;
            }
            if (sMinor == true)
            {
                Minor++;
                Built = 0;
            }
            if (sBuilt == true)
            {
                Built++;
            }
        }

        public int GetMajor()
        {
            return Major;
        }

        public int GetMinor()
        {
            return Minor;
        }

        public string ToNew()
        {
            return "\""+ Namespace + "\", \"\" + NWDConstants.NWD3Assemblies + \"/"+ Path.Replace(NWDConstants.NWD3Assemblies,"") + "\", " + Major.ToString() + ", " + Minor.ToString() + ", " + Built.ToString();
        }

        public string ToVerSem()
        {
            return Major.ToString() + "." + Minor.ToString() + "." + Built.ToString();
        }

        public override string ToString()
        {
            return Major.ToString() + "." + Minor.ToString("00") + "." + Built.ToString("000");
        }
    }
}
