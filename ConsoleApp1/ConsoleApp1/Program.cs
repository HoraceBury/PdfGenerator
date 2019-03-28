using System.Text;
using System.IO;

using DinkToPdf;
using System;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var str = GenerateHtml();
            GeneratePdf();
        }

        public static string GenerateHtml()
        {
            return Razor2Pdf.RazorRenderer.DoHtml();
        }

        public static void GeneratePdf()
        {
            var slnpath = $@"{Directory.GetCurrentDirectory()}\..\..\..\..";
            var htmlpath = $@"{slnpath}\HtmlTemplates\HTMLPage1.html";
            var pdfpath = $@"{slnpath}\PdfFiles\Azam.pdf";
            var dllpath = $@"{slnpath}\DinkNative64bit\libwkhtmltox.dll";

            var html = new StringBuilder(File.ReadAllText(htmlpath));

            var _converter = new SynchronizedConverter(new PdfTools());

            var context = new CustomAssemblyLoadContext().LoadUnmanagedLibrary(dllpath);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
                //Out = @"D:\PDFCreator\Employee_Report.pdf"  USE THIS PROPERTY TO SAVE PDF TO A PROVIDED LOCATION
            };
            
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = html.ToString(),
                //Page = "https://code-maze.com/", USE THIS PROPERTY TO GENERATE PDF CONTENT FROM AN HTML PAGE
                WebSettings = { DefaultEncoding = "utf-8" }, //, UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" },
                
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            //_converter.Convert(pdf); IF WE USE Out PROPERTY IN THE GlobalSettings CLASS, THIS IS ENOUGH FOR CONVERSION

            var file = _converter.Convert(pdf);

            File.WriteAllBytes(pdfpath, file);

            //return Ok("Successfully created PDF document.");
            //return File(file, "application/pdf", "EmployeeReport.pdf"); //USE THIS RETURN STATEMENT TO DOWNLOAD GENERATED PDF DOCUMENT
            //return File(file, "application/pdf");
        }
    }
}
