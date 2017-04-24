namespace ServiceBooking.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Version3 : DbMigration
    {
        public override void Up()
        {
            Sql(@"SET IDENTITY_INSERT dbo.Categories ON");
            Sql(@"INSERT INTO dbo.Categories (Id, Name) VALUES ('1', 'Ritual services');");
            Sql(@"INSERT INTO dbo.Categories (Id, Name) VALUES ('2', 'Cleaning');");
            Sql(@"INSERT INTO dbo.Categories (Id, Name) VALUES ('3', 'Photo and video services');");
            Sql(@"SET IDENTITY_INSERT dbo.Categories OFF");

            Sql(@"SET IDENTITY_INSERT dbo.Status ON");
            Sql(@"INSERT INTO dbo.Status (Id, Value) VALUES ('1', 'Pending confirmation');");
            Sql(@"INSERT INTO dbo.Status (Id, Value) VALUES ('2', 'Active');");
            Sql(@"INSERT INTO dbo.Status (Id, Value) VALUES ('3', 'Confirmed');");
            Sql(@"INSERT INTO dbo.Status (Id, Value) VALUES ('4', 'Completed');");
            Sql(@"INSERT INTO dbo.Status (Id, Value) VALUES ('5', 'Outstanding');");
            Sql(@"SET IDENTITY_INSERT dbo.Status OFF");

        }

        public override void Down()
        {
        }
    }
}
