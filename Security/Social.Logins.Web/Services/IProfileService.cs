namespace Social.Logins.Web.Services
{
    using Social.Logins.Web.Models;
    using System.Threading.Tasks;

    public interface IProfileService
    {
        Task CreateAsync(Profile profile);
        Task<Profile> RetrieveAsync(string username);
    }
}
