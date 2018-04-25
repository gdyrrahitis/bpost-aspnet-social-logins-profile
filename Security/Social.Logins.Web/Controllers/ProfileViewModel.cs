using System.ComponentModel.DataAnnotations;

namespace Social.Logins.Web.Controllers
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Email address is not in valid format.")]
        [Required(ErrorMessage = "Email address is required.")]
        public string Email { get; set; }

        public string Address { get; set; } = string.Empty;

        public string ReturnUrl { get; set; }
    }
}
