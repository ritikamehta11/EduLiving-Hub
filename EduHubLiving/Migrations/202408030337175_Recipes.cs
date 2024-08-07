namespace EduHubLiving.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Recipes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        IngredientID = c.Int(nullable: false, identity: true),
                        IngredientName = c.String(),
                    })
                .PrimaryKey(t => t.IngredientID);
            
            CreateTable(
                "dbo.RecipeIngredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Unit = c.String(),
                        RecipeID = c.Int(nullable: false),
                        IngredientID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ingredients", t => t.IngredientID, cascadeDelete: true)
                .ForeignKey("dbo.Recipes", t => t.RecipeID, cascadeDelete: true)
                .Index(t => t.RecipeID)
                .Index(t => t.IngredientID);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        RecipeID = c.Int(nullable: false, identity: true),
                        RecipeName = c.String(),
                        RecipeType = c.String(),
                        RecipeDescription = c.String(),
                        RecipeInstructions = c.String(),
                    })
                .PrimaryKey(t => t.RecipeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RecipeIngredients", "RecipeID", "dbo.Recipes");
            DropForeignKey("dbo.RecipeIngredients", "IngredientID", "dbo.Ingredients");
            DropIndex("dbo.RecipeIngredients", new[] { "IngredientID" });
            DropIndex("dbo.RecipeIngredients", new[] { "RecipeID" });
            DropTable("dbo.Recipes");
            DropTable("dbo.RecipeIngredients");
            DropTable("dbo.Ingredients");
        }
    }
}
