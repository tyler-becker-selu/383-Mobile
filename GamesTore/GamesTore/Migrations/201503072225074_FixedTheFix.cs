namespace GamesTore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedTheFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CartModels", "CartModel_Id", "dbo.CartModels");
            DropForeignKey("dbo.CartModels", "GameModel_Id", "dbo.GameModels");
            DropIndex("dbo.CartModels", new[] { "CartModel_Id" });
            DropIndex("dbo.CartModels", new[] { "GameModel_Id" });
            CreateTable(
                "dbo.GameModelCartModels",
                c => new
                    {
                        GameModel_Id = c.Int(nullable: false),
                        CartModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GameModel_Id, t.CartModel_Id })
                .ForeignKey("dbo.GameModels", t => t.GameModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.CartModels", t => t.CartModel_Id, cascadeDelete: true)
                .Index(t => t.GameModel_Id)
                .Index(t => t.CartModel_Id);
            
            DropColumn("dbo.CartModels", "CartModel_Id");
            DropColumn("dbo.CartModels", "GameModel_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CartModels", "GameModel_Id", c => c.Int());
            AddColumn("dbo.CartModels", "CartModel_Id", c => c.Int());
            DropForeignKey("dbo.GameModelCartModels", "CartModel_Id", "dbo.CartModels");
            DropForeignKey("dbo.GameModelCartModels", "GameModel_Id", "dbo.GameModels");
            DropIndex("dbo.GameModelCartModels", new[] { "CartModel_Id" });
            DropIndex("dbo.GameModelCartModels", new[] { "GameModel_Id" });
            DropTable("dbo.GameModelCartModels");
            CreateIndex("dbo.CartModels", "GameModel_Id");
            CreateIndex("dbo.CartModels", "CartModel_Id");
            AddForeignKey("dbo.CartModels", "GameModel_Id", "dbo.GameModels", "Id");
            AddForeignKey("dbo.CartModels", "CartModel_Id", "dbo.CartModels", "Id");
        }
    }
}
