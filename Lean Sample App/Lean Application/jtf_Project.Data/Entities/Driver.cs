using jtf_Project.Data.Enums;
using jtf_Project.Data.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace jtf_Project.Data.Entities
{
    public class Driver
    {

        public Driver()
        {
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.MiddleName = string.Empty;
            this.BirthDate = DateTime.Now;
        }
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public GenderEnum Gender { get; set; }
        public int Age { get; set; }
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

        [ForeignKey("ImageId")]
        public UserImage UserImage { get; set; }

        public void SetAge()
        {
            this.Age = DateTime.Now.Year - BirthDate.Year;
        }
        [NotMapped]
        public string FullName
        {
            get
            {
                return (!string.IsNullOrEmpty(this.FirstName) ? this.FirstName.First().ToString().ToUpper() + string.Join("", this.FirstName.Skip(1)) : string.Empty) + " "
                    + (!string.IsNullOrEmpty(this.MiddleName) ? this.MiddleName.Substring(0, 1) : string.Empty)
                    + ". " + this.LastName;
            }
        }
        [NotMapped]
        public string GenderString
        {
            get
            {
                return EnumHelper.GetName((GenderEnum)this.Gender);
            }
        }

        [NotMapped]
        public SelectList GenderSelectlist
        {
            get
            {
                return EnumHelper.ToSelectList((GenderEnum)this.Gender);
            }
        }
    }
}
