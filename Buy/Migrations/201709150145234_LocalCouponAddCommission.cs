namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocalCouponAddCommission : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LocalCoupons", "Commission", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LocalCoupons", "Commission");
        }
    }
}
