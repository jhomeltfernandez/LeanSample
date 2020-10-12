using jtf_Project.Admin.Models.ViewModels;
using jtf_Project.Data;
using jtf_Project.Data.Entities;
using jtf_Project.Data.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace jtf_Project.Admin.Models
{
    public static class ModelFactory
    {
        public static OtherExpenseReportModel ToReportModel(OtherExpence model)
        {
            return new OtherExpenseReportModel() 
            { 
                Name = model.Name,
                Amount = model.Amount,
                SaleId = model.SaleId.HasValue?model.SaleId.Value:0
            };
        }



    //    public static UserVModel ToViewModel(this ApplicationUser model)
    //    { 
    //        var data = new UserVModel()
    //        {
    //            Id = model.Id,
    //            ProfileId = model.ProfileId.Value,
    //            Status = model.Status??false,
    //            EmailAddress = model.Email,
    //            ContactNumber = model.PhoneNumber,
    //            Role = string.Join(", ",Roles.GetRolesForUser(model.UserName))
    //        };

    //        if (!model.Profile.isNull())
    //        {
    //            data.Profile = model.Profile.ToViewModel();
    //        }
    //        else
    //        {
    //            using (UnitOfWork Context = new UnitOfWork())
    //            {
    //                int profileId = model.ProfileId??0;

    //                data.Profile = Context.UserProfileRepo.GetByID(profileId).ToViewModel();
    //            }
    //        }

    //        data.DisplayTable = data.ToTableViewModel();

    //        return data;
    //    }


    //    public static UserProfileTableVModel ToTableViewModel(this UserVModel model)
    //    {
    //        return new UserProfileTableVModel()
    //        {
    //            Id = model.Id,
    //            Fullname = model.Profile.FullName,
    //            Username = model.UserName,
    //            EmailAddress = model.EmailAddress,
    //            Age = model.Profile.Age.Value,
    //            ContactNumber = model.ContactNumber,
    //            Address = model.Profile.Address,
    //            Gender = model.Profile.Gender.GetName(),
    //            Status = model.StatusString
    //        };
    //    }

    //    public static UserProfile ToViewModel(this UserProfileVModel model)
    //    {
    //        return new UserProfile()
    //            {
    //                Id = model.Id,
    //                FirstName = model.FirstName,
    //                MiddleName = model.MiddleName,
    //                LastName = model.LastName,
    //                Gender = model.Gender,
    //                BirthDate = model.BirthDate,
    //                Religion = model.Religion,
    //                Address = model.Address,
    //                Occupation = model.Occupation,
    //                ImageId = model.ImageId
    //            };
    //    }
    //    public static UserProfileVModel ToViewModel(this UserProfile model)
    //    {
    //        var data = new UserProfileVModel()
    //        {
    //            Id = model.Id,
    //            FirstName = model.FirstName,
    //            MiddleName = model.MiddleName,
    //            LastName = model.LastName,
    //            Gender = model.Gender,
    //            GenderString = model.Gender.GetName(),
    //            Age = model.Age,
    //            BirthDate = model.BirthDate,
    //            Religion = model.Religion,
    //            Address = model.Address,
    //            Occupation = model.Occupation,
    //            ImageId = model.ImageId
    //        };

    //        if (!model.UserImage.isNull())
    //        {
    //            data.UserImage = model.UserImage.ToViewModel();
    //        }

    //        return data;
    //    }


    //    public static UserImageVModel ToViewModel(this UserImage model)
    //    {
    //        var data = new UserImageVModel()
    //        {
    //            Id = model.Id,
    //            Name = model.Name,
    //            FileType = model.FileType,
    //            ImageDirectoryId = model.ImageDirectoryId
    //        };

    //        if (!model.UserImageDirectory.isNull())
    //        {
    //            data.UserImageDirectory = model.UserImageDirectory.ToViewModel();
    //        }

    //        return data;
    //    }

    //    public static UserImage ToEntity(this UserImageVModel model)
    //    {
    //        return new UserImage()
    //        {
    //            Id = model.Id,
    //            Name = model.Name,
    //            FileType = model.FileType,
    //            ImageDirectoryId = model.ImageDirectoryId.HasValue ? model.ImageDirectoryId.Value 
    //                    : int.Parse(ConfigurationManager.AppSettings["DefaultUserImageDirectory"])
    //        };
    //    }



    //    public static UserImageDirectoryVModel ToViewModel(this UserImageDirectory model)
    //    {
    //        var data = new UserImageDirectoryVModel()
    //        {
    //            Id = model.Id,
    //            Name = model.Name,
    //            Path = model.Path,
    //            Modified = model.Modified,
    //            Created = model.Created,
    //            UserId = model.UserId
    //        };
    //        return data;
    //    }
    //    public static UserImageDirectory ToEntity(this UserImageDirectoryVModel model)
    //    {
    //        return new UserImageDirectory{
    //            Id = model.Id,
    //            Name = model.Name,
    //            Path = model.Path,
    //            Modified = DateTime.Now,
    //            Created = model.Created.HasValue?model.Created.Value:DateTime.Now,
    //            UserId = model.UserId
    //        };
    //    }
    }
}