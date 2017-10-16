namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChildProxyApply : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChildProxyApplies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProxyID = c.String(),
                        UserID = c.String(),
                        State = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        CheckDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ChildProxyApplies");
        }
    }
}
