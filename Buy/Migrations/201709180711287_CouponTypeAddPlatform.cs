namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CouponTypeAddPlatform : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CouponTypes", "Platform", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CouponTypes", "Platform");
        }
    }
}
