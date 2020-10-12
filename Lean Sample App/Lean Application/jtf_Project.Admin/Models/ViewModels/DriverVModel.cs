using jtf_Project.Data.Entities;
using jtf_Project.Data.Enums;
using jtf_Project.Data.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jtf_Project.Admin.Models.ViewModels
{
    public class DriverVModel
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public GenderEnum Gender { get; set; }

        [Display(Name = "Birthdate")]
        [MinimumAge(18, ErrorMessage = "Your age must be 18 or above.")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}",
                        ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        //Optional
        public string Address { get; set; }
        //Act as Primary Photo
        public int? ImageId { get; set; }
        public string ContactNumber { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public Nullable<DateTime> DateHired { get; set; }
        public Nullable<DateTime> DateOut { get; set; }
        public Nullable<DateTime> Created { get; set; }
        public Nullable<DateTime> Modified { get; set; }
        public UserImage UserImage { get; set; }

        public string FullName { get; set; }
        public string GenderString { get; set; }
        public SelectList GenderSelectlist
        {
            get
            {
                return EnumHelper.ToSelectList((GenderEnum)this.Gender);
            }
        }


        public string GetAge()
        {
            return (DateTime.Now.Year - this.BirthDate.Year).ToString();
        }


        public HttpPostedFileBase UserPhoto { get; set; }
    }
}