using Microsoft.Office;
using Microsoft.Office.Interop.PowerPoint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
//using Microsoft.Office.Interop;
//using Microsoft.Office.Interop.PowerPoint;

namespace ChallengerApp.Utilities
{
    public class FileUploader
    {
        //public string guid { get; set; }
        //Convert the pdf into Single Image.
        public static void ConvertSingleImage(string filename,string outputDir)
        {
            PdfToImage.PDFConvert pdfConverter = new PdfToImage.PDFConvert();

            try
            {
                pdfConverter.RenderingThreads = 5;
                pdfConverter.TextAlphaBit = 4;
                pdfConverter.OutputToMultipleFile = false;
                pdfConverter.FirstPageToConvert = -1;
                pdfConverter.LastPageToConvert = -1;
                pdfConverter.FitPage = false;
                pdfConverter.JPEGQuality = 10;
                pdfConverter.OutputFormat = "jpeg";
                
                System.IO.FileInfo input = new FileInfo(filename);
                string[] str = input.Name.Split('.');
                string output = outputDir;
                while (System.IO.File.Exists(output))
                {
                    File.Delete(output);
                    output = outputDir;
                }

                pdfConverter.Convert(input.FullName, output);
            }

            catch (Exception ex)
            {

            }
            return;
        }

        //Method to convert the Excel file into Pdf
        public static bool ExportWorkbookToPdf(string workbookPath, string outputPath)
        {
           // If either required string is null or empty, stop and bail out
            if (string.IsNullOrEmpty(workbookPath) || string.IsNullOrEmpty(outputPath))
            {
                return false;
            }
            // Create COM Objects
            Microsoft.Office.Interop.Excel.Application excelApplication;
            Microsoft.Office.Interop.Excel.Workbook excelWorkbook;

            excelApplication = new Microsoft.Office.Interop.Excel.Application();        // Create new instance of Excel
            excelApplication.ScreenUpdating = false;                                    // Make the process invisible to the user
            excelApplication.DisplayAlerts = false;                                     // Make the process silent
            excelWorkbook = excelApplication.Workbooks.Open(workbookPath);          // Open the workbook that you wish to export to PDF
            // If the workbook failed to open, stop, clean up, and bail out
            if (excelWorkbook == null)
            {
                excelApplication.Quit();
                excelApplication = null;
                excelWorkbook = null;
                return false;
            }
            var exportSuccessful = true;
            try
            {
                // Call Excel's native export function (valid in Office 2007 and Office 2010, AFAIK)
                excelWorkbook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, outputPath);
            }
            catch (System.Exception ex)
            {
                // Mark the export as failed for the return value...
                exportSuccessful = false;
                // Do something with any exceptions here, if you wish...
                // MessageBox.Show...        
            }
            finally
            {
                // Close the workbook, quit the Excel, and clean up regardless of the results...
                excelWorkbook.Close();
                excelApplication.Quit();
                excelApplication = null;
                excelWorkbook = null;
            }
            return exportSuccessful;
        }

        public static bool ExportWordFileToPdf(string wordPath, string outputPath)
        {
           Microsoft.Office.Interop.Word.Document wordDocument = null;

            Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
            wordDocument = appWord.Documents.Open(wordPath);
            var exportSuccessful = true;
            try
            {
                wordDocument.ExportAsFixedFormat(outputPath, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
            }
            catch (System.Exception ex)
            {
                exportSuccessful = false;
            }
            finally
            {
                ((Microsoft.Office.Interop.Word._Document)wordDocument).Close();
                ((Microsoft.Office.Interop.Word._Application)appWord).Quit();
                appWord = null;
                wordDocument = null;
            }
            return exportSuccessful;
        }

        public static bool ExportPowerPointToImage(string inputFilePath,string outputPath)
        {
           //Microsoft.Office.Interop.PowerPoint.Application ppApp = new Microsoft.Office.Interop.PowerPoint.Application();
           // //ppApp.Visible = MsoTriState.msoTrue;               //To open the ppt file.
           // //ppApp.WindowState = PpWindowState.ppWindowMinimized;         //To minimise the opened ppt file.
           // Microsoft.Office.Interop.PowerPoint.Presentations oPresSet = ppApp.Presentations;
           // Microsoft.Office.Interop.PowerPoint._Presentation oPres = oPresSet.Open(inputFilePath,
           //             Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse,
           //             Microsoft.Office.Core.MsoTriState.msoFalse);
           // //ppApp.ShowWindowsInTaskbar = Microsoft.Office.Core.MsoTriState.msoFalse;  //Hiding the application; But it will be displayed always
           // try
           // {
           //     Microsoft.Office.Interop.PowerPoint.Slides objSlides = oPres.Slides;    //Getting all the slides
                
           //     if (oPres.Slides != null && oPres.Slides.Count > 0)
           //     {
           //         oPres.Slides[1].Export(outputPath, "jpg", 600, 400);

           //         return true;
           //     }
               
           // }
           // finally
           // {
           //     ppApp.Quit();   //Closing the Powerpoint application. Sometimes it won't work too.
           // }

            return false;
        }
    }
    
}