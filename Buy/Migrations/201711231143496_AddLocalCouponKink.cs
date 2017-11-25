namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocalCouponKink : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LocalCouponKinds",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Image = c.String(),
                        Sort = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.LocalCoupons", "KindID", c => c.Int(nullable: false));
            AddColumn("dbo.Shops", "Remark", c => c.String());
            AddColumn("dbo.Shops", "Images", c => c.String());
            AddColumn("dbo.Shops", "PhoneNumber", c => c.String());
            AddColumn("dbo.Shops", "Lat", c => c.Double());
            AddColumn("dbo.Shops", "Lng", c => c.Double());
            AddColumn("dbo.Shops", "Address", c => c.String());
            AddColumn("dbo.Shops", "Province", c => c.String());
            AddColumn("dbo.Shops", "City", c => c.String());
            AddColumn("dbo.Shops", "District", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shops", "District");
            DropColumn("dbo.Shops", "City");
            DropColumn("dbo.Shops", "Province");
            DropColumn("dbo.Shops", "Address");
            DropColumn("dbo.Shops", "Lng");
            DropColumn("dbo.Shops", "Lat");
            DropColumn("dbo.Shops", "PhoneNumber");
            DropColumn("dbo.Shops", "Images");
            DropColumn("dbo.Shops", "Remark");
            DropColumn("dbo.LocalCoupons", "KindID");
            DropTable("dbo.LocalCouponKinds");
        }
    }
}
