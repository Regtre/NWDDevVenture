using Microsoft.AspNetCore.Mvc;
using NWDWebRuntime.Models.Enums;
using NWDWebRuntime.Tools;
using NWDWebStandard.Controllers;

//using Wkhtmltopdf.NetCore;

namespace NWDWebPlayerDemo.Controllers;
public class WWWTestPdfController : NWDBasicController<WWWTestPdfController>
{
    //readonly IGeneratePdf _generatePdf;
    //private readonly NWDViewRenderer? view;
    
    // public WWWTestPdfController(NWDViewRenderer view, IGeneratePdf generatePdf)
    // {
    //     this.view = view;
    //     _generatePdf = generatePdf;
    // }
    //
    public IActionResult Index()
    {
        PageInformation.PageStyle = NWDPageContainer.IndependentPage;
        // return View("~/Views/Shared/_pdfIndex.cshtml");
        return View("~/Views/WWWTestPdf/_pdfIndex.cshtml");
    }
    
    // public FileResult pdf()
    // {
    //     PageInformation.PageStyle = NWDPageContainer.PdfPage;
    //     var tOptions = new ConvertOptions
    //     {
    //         PageSize = Wkhtmltopdf.NetCore.Options.Size.A4,
    //         HeaderSpacing = 20,
    //         FooterSpacing = 20,
    //     };
    //     _generatePdf.SetConvertOptions(tOptions);
    //     //string tFileContent = view.Render(HttpContext,"~/Views/Shared/_pdfIndex.cshtml", ViewData);
    //     string tFileContent = view.Render(HttpContext,"~/Views/WWWTestPdf/_pdfIndex.cshtml", ViewData);
    //     var pdf = _generatePdf.GetPDF(tFileContent);
    //     var pdfStream = new System.IO.MemoryStream();
    //     pdfStream.Write(pdf, 0, pdf.Length);
    //     pdfStream.Position = 0;
    //     byte[] docBytes = pdfStream.ToArray();
    //     return File(docBytes, "text/pdf", "test.pdf");
    // }
}