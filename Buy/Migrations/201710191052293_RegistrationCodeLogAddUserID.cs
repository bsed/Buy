namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegistrationCodeLogAddUserID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RegistrationCodeLogs", "UserID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RegistrationCodeLogs", "UserID");
        }
    }
}
