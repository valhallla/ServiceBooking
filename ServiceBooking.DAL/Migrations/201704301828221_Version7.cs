namespace ServiceBooking.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ExceptionDetails", "Message", c => c.String());
            AlterColumn("dbo.ExceptionDetails", "Url", c => c.String());
            AlterColumn("dbo.ExceptionDetails", "UrlReferrere", c => c.String());
            AlterColumn("dbo.ExceptionDetails", "StackTrace", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ExceptionDetails", "StackTrace", c => c.String(nullable: false));
            AlterColumn("dbo.ExceptionDetails", "UrlReferrere", c => c.String(nullable: false));
            AlterColumn("dbo.ExceptionDetails", "Url", c => c.String(nullable: false));
            AlterColumn("dbo.ExceptionDetails", "Message", c => c.String(nullable: false));
        }
    }
}
