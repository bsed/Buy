namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCouponUserAndKeywords : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CouponUsers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CouponID = c.Int(nullable: false),
                        UserID = c.String(),
                        Link = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Coupons", t => t.CouponID, cascadeDelete: true)
                .Index(t => t.CouponID);
            
            CreateTable(
                "dbo.Keywords",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Word = c.String(),
                        CouponNameCount = c.Int(nullable: false),
                        SearchCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CouponUsers", "CouponID", "dbo.Coupons");
            DropIndex("dbo.CouponUsers", new[] { "CouponID" });
            DropTable("dbo.Keywords");
            DropTable("dbo.CouponUsers");
        }
    }
}
