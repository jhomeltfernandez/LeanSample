using jtf_Project.Data.Enums;
using jtf_Project.Data.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace jtf_Project.Data.Entities
{
    public class UserProfile : Person
    {
        public UserProfile()
        {
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.MiddleName = string.Empty;
            this.BirthDate = DateTime.Now;
        }
        public int Id { get; set; }


        public Nullable<DateTime> Created { get; set; }
        public Nullable<DateTime> Modified { get; set; }
    }

    public class UserImage
    {
        public UserImage()
        {
            this.Name = ConfigurationManager.AppSettings["DefaultUserImage"].ToString();
            this.Path = ConfigurationManager.AppSettings["DefaultUserImageDirectory"].ToString();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public int? ImageDirectoryId { get; set; }
        public string Path { get; set; }

        [ForeignKey("ImageDirectoryId")]
        public UserImageDirectory UserImageDirectory { get; set; }

        public string ImageLocation
        {
            get
            {
                return System.IO.Path.Combine(this.Path, this.Name);
            }
        }
    }

    public class UserImageDirectory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }

    public class LayoutSetting
    {
        public int Id { get; set; }
        public bool FixedLayout { get; set; }
        public bool BoxedLayout { get; set; }
        public bool ToggleSideBar { get; set; }
        public bool SideBarExpandOnHover { get; set; }
        public bool ToggleRightSideBarSlide { get; set; }
        public bool ToggleRightSideBarSkin { get; set; }
        public int Skin { get; set; }
    }

    public abstract class Person
    {
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


        [ForeignKey("ImageId")]
        public virtual UserImage UserImage { get; set; }

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
