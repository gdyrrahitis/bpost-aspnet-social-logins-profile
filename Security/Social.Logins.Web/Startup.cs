namespace Social.Logins.Web
{
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Social.Logins.Web.Constants;
    using Social.Logins.Web.Services;

    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IProfileService, ProfileService>();

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = TemporaryAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddFacebook(options =>
            {
                options.AppId = "163258761152890";
                options.AppSecret = "50b5d2346000da7f4b86c3b7542f8ef6";
            })
            .AddTwitter(options =>
            {
                options.ConsumerKey = "3UmDhvlKAZCRGNRidl3XSTiw4";
                options.ConsumerSecret = "kXbPJ3krIdpMNYB31HpFKrhpkzOQ4MvUt3jCKTS7hjviVzNpiB";
            })
            .AddGitHub(options =>
            {
                options.ClientId = "96034b15ad9b9e0d1d6d";
                options.ClientSecret = "0d7fe6b6ca83272a04aace557689beac33a995b0";
            })
            .AddCookie(options => options.LoginPath = "/auth/signin")
            .AddCookie(TemporaryAuthenticationDefaults.AuthenticationScheme);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
