namespace ServiceBooking.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version5 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Pictures", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pictures", "Name", c => c.String(nullable: false));
        }
    }
}
