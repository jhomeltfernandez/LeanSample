using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jtf_Project.Admin.Models.ViewModels
{
    public class UserImageDirectoryVModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string UserId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}