namespace RegStaff.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RegStaff.Models.PersonsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "RegStaff.Models.Persons";
        }

        protected override void Seed(RegStaff.Models.PersonsContext context)
        {
            //context.UserProfiles.AddOrUpdate(
            //    p => p.UserName,
            //    new Models.UserProfile { UserName = "RTretyak" }
            //    );
            //context.webpages_Roles.AddOrUpdate(
            //    p => p.RoleName,
            //    new Models.webpages_Roles { RoleName = "Admin" },
            //    new Models.webpages_Roles { RoleName = "HR" },
            //    new Models.webpages_Roles { RoleName = "SV" },
            //    new Models.webpages_Roles { RoleName = "Trainer" }
            //    );
            //context.webpages_UserinRoles.AddOrUpdate(
            //    new Models.webpages_UserinRoles { RoleId = 1, UserID = 1 }
            //    );
        }
    }
}
