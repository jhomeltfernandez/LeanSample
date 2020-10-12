using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jtf_Project.Admin.Models.ViewModels
{
    public class UserImageVModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public Nullable<int> ImageDirectoryId { get; set; }
        public UserImageDirectoryVModel UserImageDirectory { get; set; }
    }
}