namespace GamesTore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CheckoutReady = c.Boolean(nullable: false),
                        User_Id = c.Int(nullable: false),
                        User_Id1 = c.Int(),
                        GameModel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserModels", t => t.User_Id1)
                .ForeignKey("dbo.GameModels", t => t.GameModel_Id)
                .Index(t => t.User_Id1)
                .Index(t => t.GameModel_Id);
            
            CreateTable(
                "dbo.UserModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        ApiKey = c.String(),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SalesModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SalesDate = c.DateTime(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cart_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                        Cart_Id1 = c.Int(),
                        Employee_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CartModels", t => t.Cart_Id1)
                .ForeignKey("dbo.UserModels", t => t.Employee_Id)
                .Index(t => t.Cart_Id1)
                .Index(t => t.Employee_Id);
            
            CreateTable(
                "dbo.GameModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameName = c.String(),
                        ReleaseDate = c.DateTime(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InventoryStock = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GenreModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GenreType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TagModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TagName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GenreModelGameModels",
                c => new
                    {
                        GenreModel_Id = c.Int(nullable: false),
                        GameModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GenreModel_Id, t.GameModel_Id })
                .ForeignKey("dbo.GenreModels", t => t.GenreModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.GameModels", t => t.GameModel_Id, cascadeDelete: true)
                .Index(t => t.GenreModel_Id)
                .Index(t => t.GameModel_Id);
            
            CreateTable(
                "dbo.TagModelGameModels",
                c => new
                    {
                        TagModel_Id = c.Int(nullable: false),
                        GameModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TagModel_Id, t.GameModel_Id })
                .ForeignKey("dbo.TagModels", t => t.TagModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.GameModels", t => t.GameModel_Id, cascadeDelete: true)
                .Index(t => t.TagModel_Id)
                .Index(t => t.GameModel_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TagModelGameModels", "GameModel_Id", "dbo.GameModels");
            DropForeignKey("dbo.TagModelGameModels", "TagModel_Id", "dbo.TagModels");
            DropForeignKey("dbo.GenreModelGameModels", "GameModel_Id", "dbo.GameModels");
            DropForeignKey("dbo.GenreModelGameModels", "GenreModel_Id", "dbo.GenreModels");
            DropForeignKey("dbo.CartModels", "GameModel_Id", "dbo.GameModels");
            DropForeignKey("dbo.CartModels", "User_Id1", "dbo.UserModels");
            DropForeignKey("dbo.SalesModels", "Employee_Id", "dbo.UserModels");
            DropForeignKey("dbo.SalesModels", "Cart_Id1", "dbo.CartModels");
            DropIndex("dbo.TagModelGameModels", new[] { "GameModel_Id" });
            DropIndex("dbo.TagModelGameModels", new[] { "TagModel_Id" });
            DropIndex("dbo.GenreModelGameModels", new[] { "GameModel_Id" });
            DropIndex("dbo.GenreModelGameModels", new[] { "GenreModel_Id" });
            DropIndex("dbo.SalesModels", new[] { "Employee_Id" });
            DropIndex("dbo.SalesModels", new[] { "Cart_Id1" });
            DropIndex("dbo.CartModels", new[] { "GameModel_Id" });
            DropIndex("dbo.CartModels", new[] { "User_Id1" });
            DropTable("dbo.TagModelGameModels");
            DropTable("dbo.GenreModelGameModels");
            DropTable("dbo.TagModels");
            DropTable("dbo.GenreModels");
            DropTable("dbo.GameModels");
            DropTable("dbo.SalesModels");
            DropTable("dbo.UserModels");
            DropTable("dbo.CartModels");
        }
    }
}
