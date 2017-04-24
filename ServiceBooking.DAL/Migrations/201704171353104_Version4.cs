namespace ServiceBooking.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Orders", new[] { "CategoryId" });
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Image = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "PictureId", c => c.Int());
            AddColumn("dbo.Orders", "PictureId", c => c.Int());
            AlterColumn("dbo.Orders", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "PictureId");
            CreateIndex("dbo.Orders", "CategoryId");
            CreateIndex("dbo.Orders", "PictureId");
            AddForeignKey("dbo.Orders", "PictureId", "dbo.Pictures", "Id");
            AddForeignKey("dbo.AspNetUsers", "PictureId", "dbo.Pictures", "Id");
            AddForeignKey("dbo.Orders", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.AspNetUsers", "PictureId", "dbo.Pictures");
            DropForeignKey("dbo.Orders", "PictureId", "dbo.Pictures");
            DropIndex("dbo.Orders", new[] { "PictureId" });
            DropIndex("dbo.Orders", new[] { "CategoryId" });
            DropIndex("dbo.AspNetUsers", new[] { "PictureId" });
            AlterColumn("dbo.Orders", "CategoryId", c => c.Int());
            AlterColumn("dbo.Orders", "Description", c => c.String());
            DropColumn("dbo.Orders", "PictureId");
            DropColumn("dbo.AspNetUsers", "PictureId");
            DropTable("dbo.Pictures");
            CreateIndex("dbo.Orders", "CategoryId");
            AddForeignKey("dbo.Orders", "CategoryId", "dbo.Categories", "Id");
        }
    }
}
