namespace Twitter.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using Twitter.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Twitter.Data.TwitterContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Twitter.Data.TwitterContext context)
        {
            if (!context.Users.Any())
            {
                var adminEmail = "admin@admin.com";
                var adminUsername = "admin";
                var adminFullname = "Administrator";
                var adminPassword = "admin";
                var adminRole = "Administrator";

                this.CreateAdminUser(context, adminEmail, adminUsername, adminFullname, adminPassword, adminRole);
            }
        }

        private void CreateAdminUser(TwitterContext context, string adminEmail, string adminUsername, string adminFullname, string adminPassword, string adminRole)
        {
            //create the "admin" user
            var adminUser = new User()
            {
                UserName = adminUsername,
                Fullname = adminFullname,
                Email = adminEmail
            };

            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);

            userManager.PasswordValidator = new PasswordValidator()
            {

                RequiredLength = 5,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            var userCreateResult = userManager.Create(adminUser, adminPassword);

            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            //create the "Administrator" role


            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var roleCreateResult = roleManager.Create(new IdentityRole(adminRole));

            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }

            // add the admin user to the Administrator role

            var addAdminRoleResult = userManager.AddToRole(adminUser.Id, adminRole);

            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }
        }
    }
}
