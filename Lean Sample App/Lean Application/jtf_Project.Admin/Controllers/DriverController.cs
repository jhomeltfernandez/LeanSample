using jtf_Project.Admin.Models.ViewModels;
using jtf_Project.Data;
using jtf_Project.Data.Entities;
using jtf_Project.Data.HelperModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jtf_Project.Admin.Controllers
{
    [DisplayName("Drivers")]
    public class DriverController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList()
        {
            var drivers = Context.DriverRepo.Get(s => s.Deleted == false, includeProperties: "UserImage").ToList();

            var model = new List<DriverVModel>();

            foreach (var driver in drivers)
            {
                var driverVModel = new DriverVModel()
                {
                    Id = driver.Id,
                    FirstName = driver.FirstName,
                    MiddleName = driver.MiddleName,
                    LastName = driver.LastName,
                    Gender = driver.Gender,
                    BirthDate = driver.BirthDate,
                    Address = driver.Address,
                    
                    ImageId = driver.ImageId,
                    UserImage = driver.UserImage,
                    ContactNumber = driver.ContactNumber,
                    Deleted = driver.Deleted,
                    DateHired = driver.DateHired,
                    DateOut = driver.DateOut,
                    Created = driver.Created,
                    Modified = driver.Modified,

                    FullName = driver.FullName,
                    GenderString = driver.GenderString

                };

                model.Add(driverVModel);
            }

            return PartialView("_GetList", model.ToList());
        }


        [DisplayName("Add/Edit")]
        public ActionResult CreateEdit(int id = 0)
        {
            string Title = string.Empty;
            var model = new DriverVModel();

            if (id != 0)
            {
                var driver = Context.DriverRepo.Get(s=>s.Id == id && s.Deleted == false, includeProperties:"UserImage").FirstOrDefault();

                model = new DriverVModel() {
                    Id = driver.Id,
                    FirstName = driver.FirstName,
                    MiddleName = driver.MiddleName,
                    LastName = driver.LastName,
                    Gender = driver.Gender,
                    BirthDate = driver.BirthDate,
                    Address = driver.Address,
                    ImageId = driver.ImageId,
                    ContactNumber = driver.ContactNumber,
                    DateHired = driver.DateHired,
                    DateOut = driver.DateOut,
                    UserImage = driver.UserImage,
                    FullName = driver.FullName
                };
            }
            else
            {
                model.UserImage = new UserImage();
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit(DriverVModel model)
        {
            RequestResultModel response = new RequestResultModel();

            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id > 0)
                    {
                        var toUpdate = Context.DriverRepo.GetByID(model.Id);
                        toUpdate.Id = model.Id;
                        toUpdate.FirstName = model.FirstName;
                        toUpdate.MiddleName = model.MiddleName;
                        toUpdate.LastName = model.LastName;
                        toUpdate.Gender = model.Gender;
                        toUpdate.BirthDate = model.BirthDate;
                        toUpdate.SetAge();
                        toUpdate.Address = model.Address;

                        if (model.UserPhoto != null && model.UserPhoto.ContentLength > 0)
                        {
                            toUpdate.ImageId = SaveUserImage(model.UserPhoto, "Driver " + model.LastName + model.BirthDate.Year.ToString());
                        }

                        toUpdate.ContactNumber = model.ContactNumber;
                        toUpdate.DateHired = model.DateHired;
                        toUpdate.DateOut = model.DateOut;
                        toUpdate.Modified = DateTime.Now;

                        Context.DriverRepo.Update(toUpdate);
                        Context.Save();

                        response.Message = GetMessage(Data.Enums.ResponseMessage.Update);
                        response.ReturnId = toUpdate.Id.ToString();
                    }
                    else
                    {
                        var toInsert = new Driver();
                        toInsert.ContactNumber = model.ContactNumber;
                        toInsert.Id = model.Id;
                        toInsert.FirstName = model.FirstName;
                        toInsert.MiddleName = model.MiddleName;
                        toInsert.LastName = model.LastName;
                        toInsert.Gender = model.Gender;
                        toInsert.BirthDate = model.BirthDate;
                        toInsert.SetAge();
                        toInsert.Address = model.Address;
                        toInsert.ImageId = model.ImageId;
                        toInsert.Deleted = false;
                        toInsert.ImageId = 1;

                        if (model.UserPhoto != null && model.UserPhoto.ContentLength > 0)
                        {
                            toInsert.ImageId = SaveUserImage(model.UserPhoto, "Driver " + model.LastName + model.BirthDate.Year.ToString());
                        }
                        toInsert.ContactNumber = model.ContactNumber;
                        toInsert.DateHired = model.DateHired;
                        toInsert.DateOut = model.DateOut;
                        toInsert.Modified = DateTime.Now;
                        toInsert.Created = DateTime.Now;

                        Context.DriverRepo.Insert(toInsert);
                        Context.Save();

                        response.Message = GetMessage(Data.Enums.ResponseMessage.Save);
                        response.ReturnId = toInsert.Id.ToString();
                    }

                    return ReturnSuccessResponse(response);
                }
                else
                {
                    response.Message = GetValidationErrors();
                }
            }
            catch (Exception e)
            {

                response.Message = GetValidationErrors();
            }

            return ReturnErrorResponse(response);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            RequestResultModel response = new RequestResultModel();

            try
            {
                var driver = Context.DriverRepo.GetByID(id);

                if (driver != null)
                {
                    driver.Deleted = true;

                    Context.DriverRepo.Update(driver);
                    Context.Save();
                    response.Message = GetMessage(Data.Enums.ResponseMessage.Delete);
                    return ReturnSuccessResponse(response);
                }

            }
            catch (Exception e)
            {

                response.Message = GetValidationErrors();
            }

            return ReturnErrorResponse(response);
        }




        private int? SaveUserImage(HttpPostedFileBase img, string username)
        {
            int? returnId = null;

            string defaultUserImagePath = ConfigurationManager.AppSettings["DefaultUserImageDirectory"].ToString();
            string defaultUserImage = ConfigurationManager.AppSettings["DefaultUserImage"].ToString();
            string userImageDirectory = Server.MapPath(defaultUserImagePath);

            string destinationDir = Path.Combine(userImageDirectory, username);

            var image = new UserImage();

            if (img != null && img.ContentLength > 0)
            {
                string imagePath = SaveImage(img, destinationDir);

                image.Name = SaveImage(img, destinationDir);
                image.Path = defaultUserImagePath + "/" + username;
            }
            else
            {
                return returnId;
            }
            Context.UserImageRepo.Insert(image);
            Context.Save();

            return image.Id;
        }

        private int InsertProfile(UserProfile profile)
        {
            profile.Created = DateTime.Now;
            profile.Modified = DateTime.Now;
            Context.UserProfileRepo.Insert(profile);
            Context.Save();

            return profile.Id;
        }

        private void UpdateProfile(UserProfile profile)
        {
            profile.Modified = DateTime.Now;
            Context.UserProfileRepo.Update(profile);
            Context.Save();
        }
    }
}