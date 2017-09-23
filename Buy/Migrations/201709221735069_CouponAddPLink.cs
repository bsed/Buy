namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CouponAddPLink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coupons", "PLink", c => c.String());
            DropColumn("dbo.Coupons", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Coupons", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Coupons", "PLink");
        }
    }
}
