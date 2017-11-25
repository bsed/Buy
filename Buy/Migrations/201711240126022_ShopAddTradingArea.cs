namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShopAddTradingArea : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shops", "TradingArea", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shops", "TradingArea");
        }
    }
}
