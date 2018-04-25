namespace Social.Logins.Web.Controllers
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Mvc;
    using Social.Logins.Web.Constants;
    using Social.Logins.Web.Models;
    using Social.Logins.Web.Services;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Route("auth")]
    public class AuthenticationController : Controller
    {
        private readonly IProfileService _service;

        public AuthenticationController(IProfileService service) =>
            _service = service;

        [Route("signin")]
        public async Task<IActionResult> SignIn()
        {
            var result = await HttpContext.AuthenticateAsync(TemporaryAuthenticationDefaults.AuthenticationScheme);
            if (result.Succeeded)
            {
                return RedirectToAction("Profile");
            }

            var vm = new SignInViewModel();
            return View(vm);
        }

        [Route("signin/{provider}")]
        public IActionResult SignIn(string provider, string returnUrl = null)
        {
            var profileUrl = !string.IsNullOrWhiteSpace(returnUrl) ?
                $"{Url.Action("Profile")}?returnUrl={returnUrl}" :
                Url.Action("Profile");

            return Challenge(new AuthenticationProperties { RedirectUri = profileUrl }, provider);
        }

        [Route("profile")]
        public async Task<IActionResult> Profile(string returnUrl = null)
        {
            var result = await HttpContext.AuthenticateAsync(TemporaryAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                return RedirectToAction("SignIn");
            }

            var username = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _service.RetrieveAsync(username);

            if (profile != null)
            {
                return await SignInUserAsync(profile, returnUrl);
            }

            var vm = new ProfileViewModel
            {
                UserName = result.Principal.Identity.Name,
                Email = result.Principal.FindFirst(ClaimTypes.Email)?.Value,
                Address = result.Principal.FindFirst(ClaimTypes.StreetAddress)?.Value,
                ReturnUrl = returnUrl
            };

            return View(vm);
        }

        private async Task<IActionResult> SignInUserAsync(Profile profile, string returnUrl)
        {
            await HttpContext.SignOutAsync(TemporaryAuthenticationDefaults.AuthenticationScheme);
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, profile.UserName),
                new Claim(ClaimTypes.Name, profile.UserName),
                new Claim(ClaimTypes.Email, profile.Email)
            };

            if (!string.IsNullOrWhiteSpace(profile.Address)) {
                claims.Add(new Claim(ClaimTypes.StreetAddress, profile.Address));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl);
        }

        [Route("profile")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var profile = new Profile
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Address = model.Address
                };
                await _service.CreateAsync(profile);
                return await SignInUserAsync(profile, model.ReturnUrl);
            }

            return View(model);
        }

        [Route("signout")]
        public IActionResult SignOutTemp()
        {
            return View();
        }

        [Route("signout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}