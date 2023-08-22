using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace NWDIdemobi.Configuration
{
    public class NWDIdemobiConfigureOptions : IPostConfigureOptions<StaticFileOptions>
    { 
        const string _BASE_PATH = "wwwroot";
        private IWebHostEnvironment Environment { get; }
        public NWDIdemobiConfigureOptions(IWebHostEnvironment sEnvironment)
        {
            Environment = sEnvironment;
        }
        public void PostConfigure(string? sName, StaticFileOptions sOptions)
        {
            sName = sName ?? throw new ArgumentNullException(nameof(sName));
            sOptions = sOptions ?? throw new ArgumentNullException(nameof(sOptions));

            sOptions.ContentTypeProvider = sOptions.ContentTypeProvider ?? new FileExtensionContentTypeProvider();
            if (sOptions.FileProvider == null && Environment.WebRootFileProvider == null)
            {
                throw new InvalidOperationException("Missing FileProvider.");
            }
            sOptions.FileProvider = sOptions.FileProvider ?? Environment.WebRootFileProvider;
            var tFilesProvider = new ManifestEmbeddedFileProvider(GetType().Assembly, _BASE_PATH);
            sOptions.FileProvider = new CompositeFileProvider(sOptions.FileProvider, tFilesProvider);
        }
    }
}