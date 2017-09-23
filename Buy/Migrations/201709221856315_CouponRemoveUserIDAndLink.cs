namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CouponRemoveUserIDAndLink : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Coupons", "UserID");
            DropColumn("dbo.Coupons", "Link");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Coupons", "Link", c => c.String());
            AddColumn("dbo.Coupons", "UserID", c => c.String());
        }
    }
}
