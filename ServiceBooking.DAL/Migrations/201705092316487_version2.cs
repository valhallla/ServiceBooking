namespace ServiceBooking.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version2 : DbMigration
    {
        public override void Up()
        {
            Sql(@"SET IDENTITY_INSERT dbo.AspNetRoles ON");
            Sql(@"INSERT INTO dbo.AspNetRoles (Id, Name) VALUES ('1', 'admin');");
            Sql(@"INSERT INTO dbo.AspNetRoles (Id, Name) VALUES ('2', 'user');");
            Sql(@"SET IDENTITY_INSERT dbo.AspNetRoles OFF");

            Sql(@"SET IDENTITY_INSERT dbo.AspNetUsers ON");
            Sql(@"INSERT INTO dbo.AspNetUsers (Id, Name, Surname, IsPerformer, Email, EmailConfirmed, PasswordHash, SecurityStamp, 
                PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, UserName, IsPasswordClear, RegistrationDate, AdminStatus) 
                VALUES ('1', 'admin', 'admin', 'false', 'admin@gmail.com', 'True', 'asdasd123', 'a81cfc9ba9c6','True', 'False', 
                'False', '0', 'admin@gmail.com', 'True', '01.04.2017 00:00:00', 'false');");
            Sql(@"SET IDENTITY_INSERT dbo.AspNetUsers OFF");

            Sql(@"INSERT INTO dbo.AspNetUserRoles (UserId, RoleId) VALUES ('1', '1');");

        }

        public override void Down()
        {
        }
    }
}
