namespace GamesTore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedRedundancy : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CartModels", "User_Id1", "dbo.UserModels");
            DropIndex("dbo.CartModels", new[] { "User_Id1" });
            DropIndex("dbo.SalesModels", new[] { "Cart_Id1" });
            DropIndex("dbo.SalesModels", new[] { "Employee_Id" });
            DropColumn("dbo.SalesModels", "User_Id");
            DropColumn("dbo.SalesModels", "Cart_Id");
            RenameColumn(table: "dbo.SalesModels", name: "Employee_Id", newName: "User_Id");
            RenameColumn(table: "dbo.SalesModels", name: "Cart_Id1", newName: "Cart_Id");
            AlterColumn("dbo.SalesModels", "Cart_Id", c => c.Int());
            AlterColumn("dbo.SalesModels", "User_Id", c => c.Int());
            CreateIndex("dbo.SalesModels", "Cart_Id");
            CreateIndex("dbo.SalesModels", "User_Id");
            DropColumn("dbo.CartModels", "User_Id1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CartModels", "User_Id1", c => c.Int());
            DropIndex("dbo.SalesModels", new[] { "User_Id" });
            DropIndex("dbo.SalesModels", new[] { "Cart_Id" });
            AlterColumn("dbo.SalesModels", "User_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.SalesModels", "Cart_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.SalesModels", name: "Cart_Id", newName: "Cart_Id1");
            RenameColumn(table: "dbo.SalesModels", name: "User_Id", newName: "Employee_Id");
            AddColumn("dbo.SalesModels", "Cart_Id", c => c.Int(nullable: false));
            AddColumn("dbo.SalesModels", "User_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.SalesModels", "Employee_Id");
            CreateIndex("dbo.SalesModels", "Cart_Id1");
            CreateIndex("dbo.CartModels", "User_Id1");
            AddForeignKey("dbo.CartModels", "User_Id1", "dbo.UserModels", "Id");
        }
    }
}
