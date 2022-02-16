using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RickApps.UploadFilesMVC.Models
{
    [NotMapped]
    public class Credentials
    {
        // The value of the constant is what we put in appsettings.json
        // to identify the section.
        public const string Administrator = "Administrator";
        public string UserName { get; set; } = String.Empty;
        public string PassWord { get; set; } = String.Empty;
    }
}
