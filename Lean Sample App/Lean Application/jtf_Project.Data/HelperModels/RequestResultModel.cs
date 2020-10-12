using jtf_Project.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jtf_Project.Data.HelperModel
{
    public class RequestResultModel
    {
        public RequestResultModel() {
            this.HideInSeconds = 2500;
            this.Success = true;
        }

        public String Title { get; set; }
        public String Message { get; set; }
        public string ReturnId { get; set; }
        public string Html { get; set; }
        public bool Success { get; set; }
        public int HideInSeconds { get; set; }
        public RequestResultInfoType InfoType { 
            get {
                return !Success ? RequestResultInfoType.ErrorOrDanger : RequestResultInfoType.Success;
            } 
        }
        public string ResponseView { get; set; }
    }
}
