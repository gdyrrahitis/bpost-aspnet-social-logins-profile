namespace Social.Logins.Web.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;

    public class DetailsViewModel
    {
        public string Name { get; set; }
        public List<Claim> Claims { get; set; }
    }
}
