using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using jtf_Project.Admin.Models;
using System.ComponentModel;
using jtf_Project.Data;
using jtf_Project.Admin.Models.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using jtf_Project.Data.Entities;
using System.Collections.Generic;
using jtf_Project.Data.HelperModel;
using System.Configuration;
using System.IO;

namespace jtf_Project.Admin.Controllers
{
    [DisplayName("Users")]
    public class UserController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        ApplicationDbContext AppIdentitycontext;
        RoleStore<IdentityRole> roleStore;
        RoleManager<IdentityRole> _roleManager;

        public UserController()
        {
            this.AppIdentitycontext = new ApplicationDbContext();
            this.roleStore = new RoleStore<IdentityRole>(AppIdentitycontext);
        }

        public RoleManager<IdentityRole> RoleManager
        {
            get {
                return _roleManager ?? new RoleManager<IdentityRole>(roleStore);
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList()
        {
            var appUsers = UserManager.Users.Where(s=>s.Deleted == false).ToList();

            var model = new List<UserVModel>();

            foreach (var user in appUsers)
            {
                var item = new UserVModel();

                item.Id = user.Id;
                item.UserName = user.UserName;
                item.ContactNumber = user.PhoneNumber;
                item.Status = user.Status ?? false;
                item.ProfileId = user.ProfileId.Value;
                item.Email = user.Email;
                item.Roles = UserManager.GetRoles(user.Id).ToArray();
                item.Profile = Context.UserProfileRepo.GetByID(user.ProfileId.Value);

                model.Add(item);
            }

            return PartialView("_GetList", model);
        }


        [DisplayName("Add/Edit")]
        public ActionResult CreateEdit(string id)
        {
            string Title = string.Empty;
            var model = new UserVModel();

            if (!string.IsNullOrEmpty(id))
            {
                var user = UserManager.FindById(id);

                model.Profile = Context.UserProfileRepo.Get(s => s.Id == user.ProfileId.Value, includeProperties: "UserImage").FirstOrDefault();
                model.Id = user.Id;
                model.UserName = user.UserName;
                model.ContactNumber = user.PhoneNumber;
                model.Status = user.Status ?? false;
                model.ProfileId = user.ProfileId.Value;
                model.Email = user.Email;
                model.EmailConfirmed = user.EmailConfirmed;
                model.Roles = UserManager.GetRoles(user.Id).ToArray();
            }
            else
            {
                model.Profile = new UserProfile();
                model.Profile.UserImage = new UserImage();
            }

            model.SystemRoles = RoleManager.Roles.Select(s=>s.Name).ToList();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit(UserVModel model)
        {
            RequestResultModel response = new RequestResultModel();

            try
            {
                if (model.SystemRoles == null)
                {
                    model.SystemRoles = RoleManager.Roles.Select(s=>s.Name).ToList();
                    response.Message = "Roles is required!";
                    return ReturnErrorResponse(response);
                }

                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(model.Id))
                    {
                        var userToUpdate = UserManager.FindById(model.Id);
                        userToUpdate.Status = model.Status;
                        userToUpdate.PhoneNumber = model.ContactNumber;
                        userToUpdate.EmailConfirmed = model.EmailConfirmed;

                        UserManager.Update(userToUpdate);


                        var profile = Context.UserProfileRepo.GetByID(model.ProfileId);
                        profile.BirthDate = model.Profile.BirthDate;
                        profile.SetAge();
                        profile.Gender = model.Profile.Gender;
                        profile.Address = model.Profile.Address;
                        profile.FirstName = model.Profile.FirstName;
                        profile.MiddleName = model.Profile.MiddleName;
                        profile.LastName = model.Profile.LastName;

                        if (model.UserPhoto != null && model.UserPhoto.ContentLength > 0)
                        {
                            profile.ImageId = SaveUserImage(model.UserPhoto, model.Email);
                        }

                        UpdateUserProfile(profile);

                        string[] currentUserRoles = UserManager.GetRoles(model.Id).ToArray();

                        if (currentUserRoles != null && currentUserRoles.Count() > 0)
                        {
                            UserManager.RemoveFromRoles(userToUpdate.Id, currentUserRoles);
                        }

                        UserManager.AddToRoles(userToUpdate.Id, model.SystemRoles.ToArray());

                        response.Message = GetMessage(Data.Enums.ResponseMessage.Update);
                        response.ReturnId = userToUpdate.Id;
                    }
                    else
                    {
                        if (IsEmailAlreadyInUse(model.Email))
                        {
                            response.Message = "Email is already in use.";
                            return ReturnErrorResponse(response);
                        }

                        var profile = new UserProfile();
                        profile = model.Profile;
                        profile.SetAge();
                        profile.ImageId = 1;
                        profile.ImageId = SaveUserImage(model.UserPhoto, model.Email);

                        var hasher = new PasswordHasher();

                        var userToInsert = new ApplicationUser
                        {
                            UserName = model.Email,
                            PasswordHash = hasher.HashPassword(ConfigurationManager.AppSettings["DefaultPassword"].ToString()),
                            Email = model.Email,
                            PhoneNumber = model.ContactNumber,
                            Status = model.Status,
                            EmailConfirmed = model.EmailConfirmed,
                            Deleted = false,
                            ProfileId = InsertUserProfile(profile)
                        };


                        UserManager.Create(userToInsert);
                        UserManager.AddToRoles(userToInsert.Id, model.SystemRoles.ToArray());

                        response.Message = GetMessage(Data.Enums.ResponseMessage.Save);
                        response.ReturnId = userToInsert.Id;

                        // Send an email with this link
                        if (!userToInsert.EmailConfirmed)
                        {
                            SendConfirmationEmail(userToInsert.Id);
                        }
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


        public void SendConfirmationEmail(string userId)
        {
            var user = UserManager.FindById(userId);
            if (user != null)
            {
                string code = UserManager.GenerateEmailConfirmationToken(user.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", 
                                                new { userId = user.Id, code = code }, 
                                                protocol: Request.Url.Scheme
                                            );
                UserManager.SendEmail(user.Id, "Confirm your account", 
                    "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
            } 
        }

        //public ActionResult GetDetail(string option)
        //{
        //    var user = UserManager.FindById(User.Identity.GetUserId());
        
        //}


        [HttpPost]
        public ActionResult Delete(string id)
        {
            RequestResultModel response = new RequestResultModel();

            try
            {
                var user = UserManager.FindById(id);

                if (user != null)
                {
                    user.Deleted = true;

                    UserManager.Update(user);

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

        public ActionResult ManageRoles()
        {
            return View();
        }

        public ActionResult GetUserSpecificDetail(string opt)
        {
            string result = string.Empty;

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null && user.ProfileId.HasValue)
            {
                user.Profile = Context.UserProfileRepo.Get(s => s.Id == user.ProfileId).FirstOrDefault();

                switch (opt)
                {
                    case "Image":
                        result = user.Profile.UserImage.ImageLocation;
                        break;
                    case "BirthDate":
                        result = user.Profile.BirthDate.ToString("MMM dd, yyyy");
                        break;
                    case "Email":
                        result = user.Email;
                        break;
                    case "FullName":
                        result = user.Profile.FullName;
                        break;
                    case "Roles":
                        result = string.Join(",", UserManager.GetRoles(user.Id).ToArray());
                        break;
                    default:
                        break;
                }
            }

            return Content(result, "text/html");
        }


        private bool IsEmailAlreadyInUse(string email)
        {
            return UserManager.Users.Where(s => s.Email == email || s.UserName == email).Count() > 0 ? true : false;
        }

        private int SaveUserImage(HttpPostedFileBase img, string username)
        {
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

            Context.UserImageRepo.Insert(image);
            Context.Save();

            return image.Id;
        }

        private int InsertUserProfile(UserProfile profile)
        {
            profile.Created = DateTime.Now;
            profile.Modified = DateTime.Now;
            Context.UserProfileRepo.Insert(profile);
            Context.Save();

            return profile.Id;
        }

        private void UpdateUserProfile(UserProfile profile)
        {
            profile.Modified = DateTime.Now;
            Context.UserProfileRepo.Update(profile);
            Context.Save();
        }
    }
}