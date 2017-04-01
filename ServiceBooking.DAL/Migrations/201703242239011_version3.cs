namespace ServiceBooking.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsPasswordClear", c => c.Boolean(nullable: false));

            Sql(@"SET IDENTITY_INSERT dbo.AspNetRoles ON");
            Sql(@"INSERT INTO dbo.AspNetRoles (Id, Name) VALUES ('1', 'admin');");
            Sql(@"INSERT INTO dbo.AspNetRoles (Id, Name) VALUES ('2', 'user');");
            Sql(@"SET IDENTITY_INSERT dbo.AspNetRoles OFF");

            Sql(@"SET IDENTITY_INSERT dbo.AspNetUsers ON");
            Sql(@"INSERT INTO dbo.AspNetUsers (Id, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, 
                AccessFailedCount, UserName,IsPasswordClear,Discriminator) VALUES ('1', 'service.booking.2017@gmail.com', 'True', 
                'asdasd123', 'a81cfc9ba9c6','True', 'False', 'False', '0', 'service.booking.2017@gmail.com', 'True', 'ApplicationUser');");
            Sql(@"SET IDENTITY_INSERT dbo.AspNetUsers OFF");

            Sql(@"INSERT INTO dbo.AspNetUserRoles (UserId, RoleId) VALUES ('1', '1');");
            Sql(@"INSERT INTO dbo.AspNetUserRoles (UserId, RoleId) VALUES ('1', '2');");

        }

        public override void Down()
        {
        }
    }
}
