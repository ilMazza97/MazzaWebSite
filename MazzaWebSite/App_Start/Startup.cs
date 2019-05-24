using MazzaWebSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

namespace MazzaWebSite
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator
                    .OnValidateIdentity<ApplicationUserManager, ApplicationUser, int>(
                    validateInterval: TimeSpan.FromMinutes(30),
                    regenerateIdentityCallback: (manager, user) =>
                        user.GenerateUserIdentityAsync(manager),
                    getUserIdCallback: (id) => id.GetUserId<int>())
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);


            //var options = new FluentNHibernateStorageOptions
            //{
            //    QueuePollInterval = TimeSpan.FromSeconds(15),
            //    JobExpirationCheckInterval = TimeSpan.FromHours(1),
            //    CountersAggregateInterval = TimeSpan.FromMinutes(5),
            //    UpdateSchema = true,
            //    DashboardJobListLimit = 50000,
            //    TransactionTimeout = TimeSpan.FromMinutes(1),
            //    DefaultSchema = null // use database provider's default schema
            //};
            //var storage = FluentNHibernateStorageFactory.For(ProviderTypeEnum.MySQL, "JobsDbConnection", options);

            //GlobalConfiguration.Configuration.UseStorage(storage);

            //app.UseHangfireServer();
            //app.UseHangfireDashboard("/Hangfire", new DashboardOptions
            //{
            //    Authorization = new[]
            //    {
            //        new MazzaAuthorizationFilter()
            //    }
            //});
            //HangfireJobManager.AddHangfireJob();
        }
    }
}