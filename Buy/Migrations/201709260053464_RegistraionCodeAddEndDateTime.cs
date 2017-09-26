namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegistraionCodeAddEndDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RegistrationCodes", "ActiveEndDateTime", c => c.DateTime());
            AddColumn("dbo.RegistrationCodes", "UseEndDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RegistrationCodes", "UseEndDateTime");
            DropColumn("dbo.RegistrationCodes", "ActiveEndDateTime");
        }
    }
}
