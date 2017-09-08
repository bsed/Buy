namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClientAccessLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientAccessLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        Code = c.String(),
                        LoginDateTime = c.DateTime(nullable: false),
                        IP = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ClientAccessLogs");
        }
    }
}
