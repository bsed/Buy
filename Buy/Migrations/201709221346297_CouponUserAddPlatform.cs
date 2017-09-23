namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CouponUserAddPlatform : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CouponUsers", "Platform", c => c.Int(nullable: false));
            AddColumn("dbo.CouponUsers", "PCouponID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CouponUsers", "PCouponID");
            DropColumn("dbo.CouponUsers", "Platform");
        }
    }
}
