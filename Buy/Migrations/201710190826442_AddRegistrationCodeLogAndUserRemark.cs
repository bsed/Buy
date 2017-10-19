namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRegistrationCodeLogAndUserRemark : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RegistrationCodeLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreateDateTime = c.DateTime(nullable: false),
                        From = c.String(),
                        Count = c.Int(nullable: false),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserRemarks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        RemarkUser = c.String(),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserRemarks");
            DropTable("dbo.RegistrationCodeLogs");
        }
    }
}
