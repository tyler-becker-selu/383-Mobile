namespace GamesTore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedQuantity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GameModelCartModels", "GameModel_Id", "dbo.GameModels");
            DropForeignKey("dbo.GameModelCartModels", "CartModel_Id", "dbo.CartModels");
            DropIndex("dbo.GameModelCartModels", new[] { "GameModel_Id" });
            DropIndex("dbo.GameModelCartModels", new[] { "CartModel_Id" });
            CreateTable(
                "dbo.CartGameQuantities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Cart_Id = c.Int(),
                        Game_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CartModels", t => t.Cart_Id)
                .ForeignKey("dbo.GameModels", t => t.Game_Id)
                .Index(t => t.Cart_Id)
                .Index(t => t.Game_Id);
            
            DropTable("dbo.GameModelCartModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GameModelCartModels",
                c => new
                    {
                        GameModel_Id = c.Int(nullable: false),
                        CartModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GameModel_Id, t.CartModel_Id });
            
            DropForeignKey("dbo.CartGameQuantities", "Game_Id", "dbo.GameModels");
            DropForeignKey("dbo.CartGameQuantities", "Cart_Id", "dbo.CartModels");
            DropIndex("dbo.CartGameQuantities", new[] { "Game_Id" });
            DropIndex("dbo.CartGameQuantities", new[] { "Cart_Id" });
            DropTable("dbo.CartGameQuantities");
            CreateIndex("dbo.GameModelCartModels", "CartModel_Id");
            CreateIndex("dbo.GameModelCartModels", "GameModel_Id");
            AddForeignKey("dbo.GameModelCartModels", "CartModel_Id", "dbo.CartModels", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GameModelCartModels", "GameModel_Id", "dbo.GameModels", "Id", cascadeDelete: true);
        }
    }
}
