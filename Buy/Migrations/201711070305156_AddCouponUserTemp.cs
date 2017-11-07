namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCouponUserTemp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CouponUserTemps",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CouponID = c.Int(nullable: false),
                        UserID = c.String(),
                        Link = c.String(),
                        Platform = c.Int(nullable: false),
                        PCouponID = c.String(),
                        ProductID = c.String(),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.CouponUsers", "CreateDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CouponUsers", "CreateDateTime");
            DropTable("dbo.CouponUserTemps");
        }
    }
}
