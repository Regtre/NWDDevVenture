using NWDFoundation.Logger;
using NWDWebUploader.Models;

namespace NWDWebUploader.Managers;

public class NWDUploadFileManager
{
    public static bool UploadFile(NWDUploadFile sUploadFile)
    {
        bool rReturn = false;
        if (sUploadFile.File != null)
        {

            string rPath = sUploadFile.RootPath;
            if(sUploadFile.Category != null) { rPath = Path.Combine(sUploadFile.RootPath, sUploadFile.Category); }
            if (Directory.Exists(rPath))
            {
                string tFileName = Path.GetFileName(sUploadFile.File.FileName);
                string tFilePath = Path.Combine(rPath, tFileName);
                using (FileStream stream = new FileStream(tFilePath, FileMode.Create))
                {
                    sUploadFile.File.CopyTo(stream);
                }

                rReturn = true;
            }
        }

        return rReturn;
    }
    public static void DeleteFile(string sFilePath)
    {
        if (File.Exists(sFilePath))
        {
            try
            {
                File.Delete(sFilePath);
            }
            catch (Exception e)
            {
                NWDLogger.Exception(e);
                throw;
            }
            
        }
        
    }
}