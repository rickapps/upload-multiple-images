using System.ComponentModel.DataAnnotations;

namespace RickApps.UploadFilesMVC.ViewModels
{
    public class AuthorizeLoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}