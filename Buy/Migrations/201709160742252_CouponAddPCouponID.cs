namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CouponAddPCouponID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coupons", "PCouponID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Coupons", "PCouponID");
        }
    }
}
