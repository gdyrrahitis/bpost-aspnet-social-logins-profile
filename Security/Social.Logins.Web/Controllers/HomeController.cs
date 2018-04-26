namespace Social.Logins.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Social.Logins.Web.Models;

    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Details() => View(new DetailsViewModel
        {
            Name = User.Identity.Name,
            Claims = User.Claims.ToList()
        });

        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
