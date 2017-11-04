namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocalCouponAddTypeAndLink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LocalCoupons", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.LocalCoupons", "Link", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LocalCoupons", "Link");
            DropColumn("dbo.LocalCoupons", "Type");
        }
    }
}
