using Microsoft.AspNetCore.Http;

namespace NWDWebEmailSender.Models
{
    public class NWDWebEmailFile
    {
        #region properties

        public byte[]? FileBytes;
        public string? FileName;
        public string? FileContentType;

        #endregion

        #region constructor

        public NWDWebEmailFile()
        {

        }

        #endregion

        #region method

        public NWDWebEmailFile(IFormFile sFileForm)
        {
            if (sFileForm.Length > 0)
            {
                FileName = Path.GetFileName(sFileForm.FileName);
                FileContentType = sFileForm.ContentType;
                using (var tData = new MemoryStream())
                {
                    sFileForm.CopyTo(tData);
                    FileBytes = tData.ToArray();
                }
            }
        }

        #endregion
    }
}