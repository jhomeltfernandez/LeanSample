using jtf_Project.Data.Enums;
using jtf_Project.Data.HelperModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace jtf_Project.Data
{
    public class BaseController : Controller
    {
        protected readonly UnitOfWork Context;
        protected ModelFactory ModelFactory;
        protected decimal AMVICFeeAmount
        {
            get
            {
                return Convert.ToDecimal(ConfigurationManager.AppSettings["AMVICFeeAmount"]);
            }
        }
        protected string ThumbnailPath = "~/Uploads/Thumbnails";

        public BaseController()
        {
            Context = new UnitOfWork();
            ModelFactory = new ModelFactory();
        }

        protected bool IsValidDate(string param)
        {
            DateTime outDate = DateTime.Now;

            bool flag = DateTime.TryParse(param, out outDate);

            return flag;
        }


        protected override void Dispose(bool disposing)
        {
            Context.Dispose();
            base.Dispose(disposing);
        }

        protected string GetValidationErrors()
        {
            var allErrors = ModelState.Values.SelectMany(v => v.Errors);
            StringBuilder validationErrors = new StringBuilder();
            foreach (var item in allErrors)
            {
                validationErrors.Append(String.Format("{0} {1}<br>", item.ErrorMessage, Environment.NewLine));
            }

            return validationErrors.ToString();
        }

        protected string GetMessage(string type)
        {
            string errorMessage = "";
            switch (type)
            {
                case "error":
                    errorMessage = "An error occured while processing your request!";
                    break;
                case "validationerror":
                    errorMessage = "Please correct any input errors and try again!";
                    break;
                case "save":
                    errorMessage = "The record has been saved.";
                    break;
                case "update":
                    errorMessage = "The record has been updated.";
                    break;
                case "delete":
                    errorMessage = "The record has been deleted.";
                    break;
                case "unblock":
                    errorMessage = "The member has been successfully unblocked.";
                    break;

                default:
                    errorMessage = "An unknown error has occured. Please contact your system administrator";
                    break;
            }

            return errorMessage;
        }

        protected string GetMessage(ResponseMessage response)
        {
            return GetMessage(EnumHelper.GetDescriptionInternal((int)response, new ResponseMessage()));
        }

        protected ActionResult ReturnErrorResponse(RequestResultModel requestModel)
        {
            requestModel.Title = "Error!";
            requestModel.Success = false;
            requestModel.Html = this.RenderPartialView(@"_RequestResultPageInlineMessage", requestModel);
            requestModel.HideInSeconds = 2;
            return Json(requestModel, JsonRequestBehavior.AllowGet);
        }

        protected ActionResult ReturnValidationErrorResponse(RequestResultModel requestModel, object data)
        {
            requestModel.Title = "Validation Error(s)!";
            requestModel.Success = false;
            requestModel.Html = this.RenderPartialView(@"_RequestResultPageInlineMessage", requestModel);
            requestModel.HideInSeconds = 2;
            return Json(new
            {
                requestModel
            }, JsonRequestBehavior.AllowGet);
        }

        protected ActionResult ReturnSuccessResponse(RequestResultModel requestModel)
        {
            requestModel.Title = "Success!";
            requestModel.Success = true;
            requestModel.Html = this.RenderPartialView(@"_RequestResultPageInlineMessage", requestModel);
            requestModel.HideInSeconds = 2;
            return Json(requestModel, JsonRequestBehavior.AllowGet);
        }

        protected virtual ActionResult DownloadFile(string fileName, string contentType, string path)
        {
            try
            {
                string pathToFile = Path.Combine(Server.MapPath(path), fileName);
                if (System.IO.File.Exists(pathToFile))
                {
                    var cd = new System.Net.Mime.ContentDisposition
                    {
                        // for example foo.bak
                        FileName = fileName,

                        // always prompt the user for downloading, set to true if you want 
                        // the browser to try to show the file inline
                        Inline = true,
                    };

                    Response.AppendHeader("Content-Disposition", cd.ToString());

                    return new FilePathResult(pathToFile, contentType);
                }

                return View("GenericErrorPage");

            }
            catch (Exception)
            {

            }

            return new EmptyResult();
        }




        protected string CreateDocumentThumbnail(string rawFileName, string fileExtension, string inputPath)
        {

            rawFileName = Guid.NewGuid().ToString() + ".jpg";
            var outputPath = Path.Combine(Server.MapPath(ThumbnailPath), rawFileName); ;

            switch (fileExtension.ToLower())
            {
                case ".pdf":
                    ChallengerApp.Utilities.FileUploader.ConvertSingleImage(inputPath, outputPath);
                    break;
                case ".xls":
                case ".xlsx":
                    string excelOutputPath = Server.MapPath("~/Uploads/" + Guid.NewGuid() + ".pdf");
                    bool success = ChallengerApp.Utilities.FileUploader.ExportWorkbookToPdf(inputPath, excelOutputPath);
                    if (success)
                    {
                        ChallengerApp.Utilities.FileUploader.ConvertSingleImage(excelOutputPath, outputPath);
                    }
                    break;
                case ".doc":
                case ".docx":
                case ".rtf":
                    string wordOutputPath = Server.MapPath("~/Uploads/" + Guid.NewGuid() + ".pdf");
                    bool wordSuccess = ChallengerApp.Utilities.FileUploader.ExportWordFileToPdf(inputPath, wordOutputPath);
                    if (wordSuccess)
                    {
                        ChallengerApp.Utilities.FileUploader.ConvertSingleImage(wordOutputPath, outputPath);
                    }
                    break;
                case ".ppt":
                case ".pptx":
                    string pptOutputPath = Path.Combine(Server.MapPath("~/Uploads/Thumbnails/"), rawFileName);
                    bool pptSuccess = ChallengerApp.Utilities.FileUploader.ExportPowerPointToImage(inputPath, pptOutputPath);
                    break;
            }

            return rawFileName;
        }

        /// <summary>
        /// Determine if file extension is an image
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        protected bool IsImage(string fileExtension)
        {
            bool isImage = false;
            if (fileExtension.Contains("jpeg") || fileExtension.Contains("jpg") || fileExtension.Contains("png") || fileExtension.Contains("bmp") || fileExtension.Contains("gif"))
            {
                isImage = true;
            }
            return isImage;
        }


        protected ActionResult ReturnModalPropertyResponse(string title, string formPartial, object model)
        {
            return Json(new
            {
                Title = title,
                Body = this.RenderPartialView(formPartial, model)
            }, JsonRequestBehavior.AllowGet);
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }


        public string SaveImage(HttpPostedFileBase image, string imagesDir)
        {

            return SaveImages(image, imagesDir);
        }

        private string SaveImages(HttpPostedFileBase image, string imagesDir)
        {
            string fileName = string.Empty;

            try
            {
                string uniqueCode = RandomCharGenerator.RandomString(5, false);
                string preFileName = uniqueCode + "_";
                bool isServerImagesDirExist = System.IO.Directory.Exists(imagesDir);

                if (!isServerImagesDirExist)
                {
                    System.IO.Directory.CreateDirectory(imagesDir);
                }

                //string destinationDir = Path.Combine(imagesDir, destinatioFolderName);

                //bool isPathDirectoryExist = System.IO.Directory.Exists(destinationDir);

                //if (!isPathDirectoryExist)
                //{
                //    System.IO.Directory.CreateDirectory(destinationDir);
                //}

                fileName = preFileName + "_" + Path.GetFileName(image.FileName);
                //string path = Path.Combine(destinationDir, fileName);

                string path = Path.Combine(imagesDir, fileName);

                image.SaveAs(path);
            }
            catch (Exception)
            {

                throw;
            }

            return fileName; ;
        }
    }
}
