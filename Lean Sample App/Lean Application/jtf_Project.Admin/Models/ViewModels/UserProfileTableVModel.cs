//using jtf_Project.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jtf_Project.Admin.Models.ViewModels
{
    public class UserProfileTableVModel
    {
        public string Id { get; set; }

        [DisplayName("Full name")]
        public string Fullname { get; set; }
        public string Username { get; set; }
        [DisplayName("Email")]
        public string EmailAddress { get; set; }
        [DisplayName("Contact#")]
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Status { get; set; }
    }
}