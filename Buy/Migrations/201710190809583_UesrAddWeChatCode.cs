namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UesrAddWeChatCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "WeChatCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "WeChatCode");
        }
    }
}
