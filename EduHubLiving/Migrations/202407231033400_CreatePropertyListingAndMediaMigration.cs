namespace EduHubLiving.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePropertyListingAndMediaMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Media",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: true),
                        RecipeId = c.Int(nullable: true),
                        UserId = c.String(nullable:true),
                        PropertyListingId = c.Int(nullable: true),
                        Disk = c.String(),
                        Tag = c.String(),
                        FileName = c.String(),
                        Extension = c.String(),
                        FileSize = c.String(),
                        CreatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PropertyListings", t => t.PropertyListingId, cascadeDelete: true)
                .Index(t => t.PropertyListingId);
            
            CreateTable(
                "dbo.PropertyListings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Slug = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NoBedRooms = c.Int(nullable: false),
                        NoBathRooms = c.Int(nullable: false),
                        SquareFootage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AvailabilityDate = c.DateTime(),
                        Description = c.String(),
                        Status = c.String(),
                        Type = c.String(),
                        Features = c.String(nullable: true),
                        LeaseTerm = c.String(),
                        LandLordPhoneNumber = c.String(nullable: true),
                        LandlordEmail = c.String(nullable: true),
                        UserId = c.String(maxLength: 128, nullable: true),
                        PublishedAt = c.DateTime(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PropertyListings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Media", "PropertyListingId", "dbo.PropertyListings");
            DropIndex("dbo.PropertyListings", new[] { "UserId" });
            DropIndex("dbo.Media", new[] { "PropertyListingId" });
            DropTable("dbo.PropertyListings");
            DropTable("dbo.Media");
        }
    }
}
