namespace GamesTore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class genreAndTagChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GenreModels", "Name", c => c.String());
            AddColumn("dbo.TagModels", "Name", c => c.String());
            DropColumn("dbo.GenreModels", "GenreType");
            DropColumn("dbo.TagModels", "TagName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TagModels", "TagName", c => c.String());
            AddColumn("dbo.GenreModels", "GenreType", c => c.String());
            DropColumn("dbo.TagModels", "Name");
            DropColumn("dbo.GenreModels", "Name");
        }
    }
}
