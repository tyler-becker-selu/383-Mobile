namespace GamesTore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForgotRelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartModels", "CartModel_Id", c => c.Int());
            CreateIndex("dbo.CartModels", "CartModel_Id");
            AddForeignKey("dbo.CartModels", "CartModel_Id", "dbo.CartModels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartModels", "CartModel_Id", "dbo.CartModels");
            DropIndex("dbo.CartModels", new[] { "CartModel_Id" });
            DropColumn("dbo.CartModels", "CartModel_Id");
        }
    }
}
