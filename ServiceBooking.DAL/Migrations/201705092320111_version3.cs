namespace ServiceBooking.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version3 : DbMigration
    {
        public override void Up()
        {
            Sql(@"SET IDENTITY_INSERT dbo.Categories ON");
            Sql(@"INSERT INTO dbo.Categories (Id, Name) VALUES ('1', 'Death Care services');");
            Sql(@"INSERT INTO dbo.Categories (Id, Name) VALUES ('2', 'Beauty and Style');");
            Sql(@"INSERT INTO dbo.Categories (Id, Name) VALUES ('3', 'Education');");
            Sql(@"INSERT INTO dbo.Categories (Id, Name) VALUES ('4', 'Auditing and accounting services');");
            Sql(@"INSERT INTO dbo.Categories (Id, Name) VALUES ('5', 'Legal services');");
            Sql(@"INSERT INTO dbo.Categories (Id, Name) VALUES ('6', 'Shipping operations');");
            Sql(@"INSERT INTO dbo.Categories (Id, Name) VALUES ('7', 'Tourism');");
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
