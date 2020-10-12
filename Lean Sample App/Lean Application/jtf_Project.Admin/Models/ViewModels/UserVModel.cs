using jtf_Project.Data.Entities;
using jtf_Project.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jtf_Project.Admin.Models.ViewModels
{
    public class UserVModel
    {


        public string Id { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DisplayName("Contact#")]
        public string ContactNumber { get; set; }

        public string[] Roles { get; set; }

        public List<string> SystemRoles { get; set; }

        public int ProfileId { get; set; }
        public bool Status { get; set; }
        [Display(Name = "Email Confirm")]
        public bool EmailConfirmed { get; set; }
        [DisplayName("Status")]
        public string StatusString { 
            get {
                if (this.Status)
                {
                    return "Active";
                }
                return "Inactive";
            } 
        }
        public UserProfile Profile { get; set; }
        public bool Deleted { get; set; }

        public HttpPostedFileBase UserPhoto { get; set; }
    }
}