namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRegistrationCode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RegistrationCodes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        OwnUser = c.String(),
                        UseTime = c.DateTime(),
                        UseUser = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RegistrationCodes");
        }
    }
}
