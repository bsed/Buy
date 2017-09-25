namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAddParentUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ParentUserID", c => c.String());
            AddColumn("dbo.AspNetUsers", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "EndDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "EndDateTime");
            DropColumn("dbo.AspNetUsers", "IsActive");
            DropColumn("dbo.AspNetUsers", "ParentUserID");
        }
    }
}
