namespace GamesTore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class droppedEmailFromCartModel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CartModels", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CartModels", "Email", c => c.Int(nullable: false));
        }
    }
}
