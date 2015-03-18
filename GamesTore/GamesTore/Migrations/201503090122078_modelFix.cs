namespace GamesTore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartModels", "Email", c => c.Int(nullable: false));
            AddColumn("dbo.CartModels", "Password", c => c.String());
            AddColumn("dbo.SalesModels", "Password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesModels", "Password");
            DropColumn("dbo.CartModels", "Password");
            DropColumn("dbo.CartModels", "Email");
        }
    }
}
