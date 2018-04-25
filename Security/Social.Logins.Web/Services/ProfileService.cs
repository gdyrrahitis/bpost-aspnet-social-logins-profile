namespace Social.Logins.Web.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Social.Logins.Web.Models;

    public class ProfileService : IProfileService
    {
        private IDictionary<string, Profile> _profiles = new Dictionary<string, Profile>();

        public Task CreateAsync(Profile profile)
        {
            _profiles.Add(profile.UserName, profile);
            return Task.CompletedTask;
        }

        public Task<Profile> RetrieveAsync(string username)
        {
            if (_profiles.TryGetValue(username, out Profile profile))
            {
                return Task.FromResult(profile);
            }

            return Task.FromResult<Profile>(null);
        }
    }
}
