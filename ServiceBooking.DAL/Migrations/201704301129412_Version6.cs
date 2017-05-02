namespace ServiceBooking.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExceptionDetails",
                c => new
                    {
                        Guid = c.Guid(nullable: false),
                        Message = c.String(nullable: false),
                        Url = c.String(nullable: false),
                        UrlReferrere = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        StackTrace = c.String(nullable: false),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Guid)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExceptionDetails", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ExceptionDetails", new[] { "UserId" });
            DropTable("dbo.ExceptionDetails");
        }
    }
}
