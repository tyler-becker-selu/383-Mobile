namespace GamesTore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedPasswordFromCartModel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CartModels", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CartModels", "Password", c => c.String());
        }
    }
}
