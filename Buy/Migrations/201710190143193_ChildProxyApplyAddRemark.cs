namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChildProxyApplyAddRemark : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChildProxyApplies", "Remark", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChildProxyApplies", "Remark");
        }
    }
}
