using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace NWDWebUploader.Models;

public class NWDUploadFile
{
    public string Category { set; get; } = string.Empty;
    public IFormFile? File { set; get; }
    public List<IFormFile> Files { set; get; } = new List<IFormFile>();
    public string RootPath { set; get; } = string.Empty;
}