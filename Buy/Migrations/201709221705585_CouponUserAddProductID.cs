namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CouponUserAddProductID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CouponUsers", "ProductID", c => c.String());
            DropColumn("dbo.Coupons", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Coupons", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.CouponUsers", "ProductID");
        }
    }
}
