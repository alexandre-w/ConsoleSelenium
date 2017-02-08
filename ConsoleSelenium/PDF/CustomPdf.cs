using System.Collections.Generic;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;
using ConsoleSelenium.Helpers;

namespace ConsoleSelenium.PDF
{
    class CustomPdf
    {

        public List<string> files { get; set; }
        public CustomPdf()
        {
            files = new List<string>();
        }

        public void concatPDF()
        {
            PdfDocument outputDocument = new PdfDocument();

            foreach (string file in files)
            {
                PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);
                int count = inputDocument.PageCount;
                for (int idx = 0; idx < count; idx++)
                {
                    PdfPage page = inputDocument.Pages[idx];
                    outputDocument.AddPage(page);
                }
            }

            outputDocument.Save(Const.pdfName);
            Process.Start(Const.pdfName);
        }


    }
}
