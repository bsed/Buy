namespace Buy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FavoriteAddType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Favorites", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Favorites", "Type");
        }
    }
}
