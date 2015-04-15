namespace GamesTore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedPasswordFromSales : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SalesModels", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SalesModels", "Password", c => c.String());
        }
    }
}
