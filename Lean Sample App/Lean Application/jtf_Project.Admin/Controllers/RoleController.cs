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
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using jtf_Project.Data.Entities;
using jtf_Project.Data.Enums;
using jtf_Project.Data.HelperModel;
using jtf_Project.Data;
using System.Data.Entity;

namespace jtf_Project.Admin.Controllers
{
    [DisplayName("Roles")]
    public class RoleController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        ApplicationDbContext AppIdentitycontext;
        RoleStore<IdentityRole> roleStore;
        RoleManager<IdentityRole> _roleManager;
        //= new RoleManager<IdentityRole>(roleStore);

        public RoleController()
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
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _GetList()
        {
            var roles = AppIdentitycontext.Roles.ToList();

            return PartialView("_GetList", roles);
        }

        public ActionResult CreateEdit(string id)
        {
            IdentityRole model;

            if (!string.IsNullOrEmpty(id))
            {
                model = RoleManager.FindByName(id);
            }
            else
            {
                model = new IdentityRole();
                model.Id = null;
            }

            return PartialView("_CreateEditForm", model);
        }
            
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit(string id, string name)
        {
            string returnMessage = string.Empty;

            if (ModelState.IsValid && !string.IsNullOrEmpty(name))
            {
                try
                {
                    if (string.IsNullOrEmpty(id))
                    {
                        if (!RoleManager.RoleExists(name))
                        {

                            RoleManager.Create(new IdentityRole(name));
                            AppIdentitycontext.SaveChanges();

                            return ReturnSuccessResponse(new RequestResultModel()
                            {
                                Message = GetMessage(ResponseMessage.Save)
                                //ResponseView = this.RenderPartialViewToString("_Getlist", roles)
                            });
                        }

                        returnMessage = "Role name already exist!";
                    }
                    else
                    {

                        if (!RoleManager.RoleExists(name))
                        {
                            RoleManager.Create(new IdentityRole(name));
                            AppIdentitycontext.SaveChanges();

                            var oldRole = RoleManager.FindByName(id);

                            var users = UserManager.Users.ToList();

                            foreach (var user in users)
                            {
                                if (UserManager.IsInRole(user.Id, oldRole.Name))
                                {

                                    UserManager.RemoveFromRole(user.Id, oldRole.Name);

                                    UserManager.AddToRole(user.Id, name);
                                }
                            }

                            RoleManager.Delete(oldRole);
                            AppIdentitycontext.SaveChanges();


                            return ReturnSuccessResponse(new RequestResultModel()
                            {
                                Message = GetMessage(ResponseMessage.Update)
                                //ResponseView = this.RenderPartialViewToString("_Getlist", roles)
                            });
                        }

                        returnMessage = "Role name already exist!";

                    }
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }


            return ReturnErrorResponse(new RequestResultModel() { Message = returnMessage  });
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                var role = RoleManager.FindByName(id);
                if (role != null)
                {

                    var users = UserManager.Users.ToList();

                    foreach (var user in users)
                    {
                        if (UserManager.IsInRole(user.Id, role.Name))
                        {
                            UserManager.RemoveFromRole(user.Id, role.Name);
                        }
                    }

                    RoleManager.Delete(role);
                    AppIdentitycontext.SaveChanges();

                    return ReturnSuccessResponse(new RequestResultModel()
                    {
                        Message = GetMessage(ResponseMessage.Delete)
                        //ResponseView = this.RenderPartialViewToString("_Getlist", roles)
                    });
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            return ReturnErrorResponse(new RequestResultModel() { 
                        Message = GetMessage(ResponseMessage.Error)
                    });;
        }


        public ActionResult GetRolesForUser(string userId)
        {
            string roles = string.Join(", ", UserManager.GetRoles(userId).ToList());
            roles = roles.Length > 18 && !string.IsNullOrEmpty(roles) ? roles.Substring(0, 18) + "..."
                : !string.IsNullOrEmpty(roles) && roles.Length <= 18 ? roles : "none";

            return Content(roles,"text/html");
        }
    }
}