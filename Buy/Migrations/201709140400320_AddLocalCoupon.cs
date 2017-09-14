namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocalCoupon : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LocalCoupons",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ShopID = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remark = c.String(),
                        Image = c.String(),
                        EndDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Shops", t => t.ShopID, cascadeDelete: true)
                .Index(t => t.ShopID);
            
            CreateTable(
                "dbo.Shops",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Logo = c.String(),
                        Sort = c.Int(nullable: false),
                        Code = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ShopMembers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        ShopID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Shops", t => t.ShopID, cascadeDelete: true)
                .Index(t => t.ShopID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShopMembers", "ShopID", "dbo.Shops");
            DropForeignKey("dbo.LocalCoupons", "ShopID", "dbo.Shops");
            DropIndex("dbo.ShopMembers", new[] { "ShopID" });
            DropIndex("dbo.LocalCoupons", new[] { "ShopID" });
            DropTable("dbo.ShopMembers");
            DropTable("dbo.Shops");
            DropTable("dbo.LocalCoupons");
        }
    }
}
