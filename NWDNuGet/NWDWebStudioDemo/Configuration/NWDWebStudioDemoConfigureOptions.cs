using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace NWDWebStudioDemo.Configuration
{
    public class NWDWebStudioDemoConfigureOptions : IPostConfigureOptions<StaticFileOptions>
    { 
        const string K_BasePath = "wwwroot";
        private IWebHostEnvironment Environment { get; }
        public NWDWebStudioDemoConfigureOptions(IWebHostEnvironment sEnvironment)
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
            var tFilesProvider = new ManifestEmbeddedFileProvider(GetType().Assembly, K_BasePath);
            sOptions.FileProvider = new CompositeFileProvider(sOptions.FileProvider, tFilesProvider);
        }
    }
}