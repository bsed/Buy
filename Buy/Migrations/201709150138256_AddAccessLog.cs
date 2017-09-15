namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccessLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IP = c.String(),
                        Type = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        Data = c.String(),
                        UserID = c.String(),
                        Source = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AccessLogs");
        }
    }
}
