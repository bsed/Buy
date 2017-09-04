namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CouponTypeAddImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CouponTypes", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CouponTypes", "Image");
        }
    }
}
