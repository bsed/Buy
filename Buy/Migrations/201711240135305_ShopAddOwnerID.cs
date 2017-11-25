namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShopAddOwnerID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shops", "OwnerID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shops", "OwnerID");
        }
    }
}
