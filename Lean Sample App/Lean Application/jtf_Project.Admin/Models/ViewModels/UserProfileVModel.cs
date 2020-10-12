using jtf_Project.Data.Entities;
using jtf_Project.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jtf_Project.Admin.Models.ViewModels
{
    public class UserProfileVModel
    {
        public string Id { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DisplayName("Contact#")]
        public string ContactNumber { get; set; }
        public string Roles { get; set; }

        public int ProfileId { get; set; }
        [Display(Name = "Email Confirm")]
        public bool EmailConfirmed { get; set; }
        [DisplayName("Status")]
        public UserProfile Profile { get; set; }

        public HttpPostedFileBase UserPhoto { get; set; }
    }
}