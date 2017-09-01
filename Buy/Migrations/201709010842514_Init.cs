namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coupons",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TypeID = c.Int(),
                        ProductType = c.String(),
                        ProductID = c.String(),
                        Platform = c.Int(nullable: false),
                        ShopName = c.String(),
                        Name = c.String(),
                        Image = c.String(),
                        Subtitle = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OriginalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Value = c.String(),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        DataJson = c.String(),
                        CreateDateTime = c.DateTime(nullable: false),
                        Sales = c.Int(nullable: false),
                        UrlLisr = c.String(),
                        Commission = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CommissionRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Left = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                        UserID = c.String(),
                        Link = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CouponTypes", t => t.TypeID)
                .Index(t => t.TypeID);
            
            CreateTable(
                "dbo.CouponTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Keyword = c.String(),
                        Sort = c.Int(nullable: false),
                        ParentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.FoodCoupons",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TypeID = c.Int(nullable: false),
                        Image = c.String(),
                        DetailImage = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        Remark = c.String(),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FoodCouponTypes", t => t.TypeID, cascadeDelete: true)
                .Index(t => t.TypeID);
            
            CreateTable(
                "dbo.FoodCouponTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Image = c.String(),
                        Sort = c.Int(nullable: false),
                        Enable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Helps",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Code = c.String(),
                        Content = c.String(),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.RoleGroups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Roles = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Type = c.Int(),
                        Group = c.String(),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SystemSettings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Key = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UpdateLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Content = c.String(),
                        CreateDateTime = c.DateTime(nullable: false),
                        Url = c.String(),
                        Ver = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Avatar = c.String(),
                        NickName = c.String(),
                        WeChatID = c.String(),
                        QRCode = c.String(),
                        RegisterDateTime = c.DateTime(nullable: false),
                        LastLoginDateTime = c.DateTime(nullable: false),
                        RoleGroupID = c.Int(),
                        UserType = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.VerificationCodes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        To = c.String(nullable: false),
                        Code = c.String(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        IP = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.FoodCoupons", "TypeID", "dbo.FoodCouponTypes");
            DropForeignKey("dbo.Coupons", "TypeID", "dbo.CouponTypes");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.FoodCoupons", new[] { "TypeID" });
            DropIndex("dbo.Coupons", new[] { "TypeID" });
            DropTable("dbo.VerificationCodes");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UpdateLogs");
            DropTable("dbo.SystemSettings");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RoleGroups");
            DropTable("dbo.Helps");
            DropTable("dbo.FoodCouponTypes");
            DropTable("dbo.FoodCoupons");
            DropTable("dbo.CouponTypes");
            DropTable("dbo.Coupons");
        }
    }
}
