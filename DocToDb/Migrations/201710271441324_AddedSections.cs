namespace DocToDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSections : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                        Section_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sections", t => t.Section_Id)
                .Index(t => t.Section_Id);
            
            CreateTable(
                "dbo.Subclasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                        Section_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Classes", t => t.Section_Id)
                .Index(t => t.Section_Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                        Subclass_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subclasses", t => t.Subclass_Id)
                .Index(t => t.Subclass_Id);
            
            CreateTable(
                "dbo.Subgroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                        Group_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                        Subgroup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subgroups", t => t.Subgroup_Id)
                .Index(t => t.Subgroup_Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                        Type_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Types", t => t.Type_Id)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.Subcategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subclasses", "Section_Id", "dbo.Classes");
            DropForeignKey("dbo.Types", "Subgroup_Id", "dbo.Subgroups");
            DropForeignKey("dbo.Categories", "Type_Id", "dbo.Types");
            DropForeignKey("dbo.Subcategories", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Subgroups", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Groups", "Subclass_Id", "dbo.Subclasses");
            DropForeignKey("dbo.Classes", "Section_Id", "dbo.Sections");
            DropIndex("dbo.Subcategories", new[] { "Category_Id" });
            DropIndex("dbo.Categories", new[] { "Type_Id" });
            DropIndex("dbo.Types", new[] { "Subgroup_Id" });
            DropIndex("dbo.Subgroups", new[] { "Group_Id" });
            DropIndex("dbo.Groups", new[] { "Subclass_Id" });
            DropIndex("dbo.Subclasses", new[] { "Section_Id" });
            DropIndex("dbo.Classes", new[] { "Section_Id" });
            DropTable("dbo.Subcategories");
            DropTable("dbo.Categories");
            DropTable("dbo.Types");
            DropTable("dbo.Subgroups");
            DropTable("dbo.Groups");
            DropTable("dbo.Subclasses");
            DropTable("dbo.Classes");
            DropTable("dbo.Sections");
        }
    }
}
