namespace Emart.Migrations.Template
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attributes",
                c => new
                    {
                        AttributeId = c.Int(nullable: false, identity: true),
                        AttributeName = c.String(),
                        AttributeValue = c.String(),
                        VendorId = c.String(),
                        ProductId = c.String(),
                    })
                .PrimaryKey(t => t.AttributeId);
            
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        BrandId = c.Int(nullable: false, identity: true),
                        BrandName = c.String(),
                    })
                .PrimaryKey(t => t.BrandId);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        CartId = c.Int(nullable: false, identity: true),
                        ProductId = c.String(),
                        userId = c.String(),
                        VendorId = c.String(),
                        Status = c.String(),
                        Quantity = c.String(),
                        ProductName = c.String(),
                        Price = c.String(),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.CartId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Eshoppers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        slider1_Text1 = c.String(),
                        slider1_Text1_color = c.String(),
                        slider1_Text2 = c.String(),
                        slider1_Text2_color = c.String(),
                        slider1_Text3 = c.String(),
                        slider1_Text3_color = c.String(),
                        slider2_Text1 = c.String(),
                        slider2_Text1_color = c.String(),
                        slider2_Text2 = c.String(),
                        slider2_Text2_color = c.String(),
                        slider2_Text3 = c.String(),
                        slider2_Text3_color = c.String(),
                        slider3_Text1 = c.String(),
                        slider3_Text1_color = c.String(),
                        slider3_Text2 = c.String(),
                        slider3_Text2_color = c.String(),
                        slider3_Text3 = c.String(),
                        slider3_Text3_color = c.String(),
                        Text1 = c.String(),
                        Text1_color = c.String(),
                        Text2 = c.String(),
                        Text2_color = c.String(),
                        Text3 = c.String(),
                        Text3_color = c.String(),
                        MobileNumber = c.Int(nullable: false),
                        Email = c.String(),
                        FacebookLink = c.String(),
                        TwitterLink = c.String(),
                        InstagramLink = c.String(),
                        GooglelusLink = c.String(),
                        Header = c.String(),
                        Title = c.String(),
                        Footer = c.String(),
                        VendorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ImageStores",
                c => new
                    {
                        ImageId = c.Int(nullable: false, identity: true),
                        ImageName = c.String(),
                        ImageByte = c.Binary(),
                        ImagePath = c.String(),
                        IsDeleted = c.Boolean(),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ImageId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        VendorId = c.String(),
                        ProductDescription = c.String(),
                        ProductName = c.String(),
                        Price = c.String(),
                        Detail = c.String(),
                        MainCategoryId = c.String(),
                        VendorCategoryId = c.String(),
                        ImagePath = c.String(),
                        ActualImagePath = c.String(),
                        ImageStore_ImageId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.ImageStores", t => t.ImageStore_ImageId)
                .Index(t => t.ImageStore_ImageId);
            
            CreateTable(
                "dbo.Templates",
                c => new
                    {
                        TemplateId = c.Int(nullable: false, identity: true),
                        TemplateName = c.String(),
                    })
                .PrimaryKey(t => t.TemplateId);
            
            CreateTable(
                "dbo.Vendors",
                c => new
                    {
                        VendorId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        VendorUserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        ConfirmPassword = c.String(),
                        TemplateId = c.Int(nullable: false),
                        UserName = c.String(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.VendorId)
                .ForeignKey("dbo.Templates", t => t.TemplateId, cascadeDelete: true)
                .Index(t => t.TemplateId);
            
            CreateTable(
                "dbo.VendorCategories",
                c => new
                    {
                        VendorCatId = c.Int(nullable: false, identity: true),
                        VendorCategoryName = c.String(),
                        VendorId = c.String(),
                    })
                .PrimaryKey(t => t.VendorCatId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vendors", "TemplateId", "dbo.Templates");
            DropForeignKey("dbo.Products", "ImageStore_ImageId", "dbo.ImageStores");
            DropIndex("dbo.Vendors", new[] { "TemplateId" });
            DropIndex("dbo.Products", new[] { "ImageStore_ImageId" });
            DropTable("dbo.VendorCategories");
            DropTable("dbo.Vendors");
            DropTable("dbo.Templates");
            DropTable("dbo.Products");
            DropTable("dbo.ImageStores");
            DropTable("dbo.Eshoppers");
            DropTable("dbo.Categories");
            DropTable("dbo.Carts");
            DropTable("dbo.Brands");
            DropTable("dbo.Attributes");
        }
    }
}
