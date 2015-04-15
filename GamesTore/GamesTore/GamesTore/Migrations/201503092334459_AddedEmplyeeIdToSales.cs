namespace GamesTore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEmplyeeIdToSales : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesModels", "EmployeeId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesModels", "EmployeeId");
        }
    }
}
