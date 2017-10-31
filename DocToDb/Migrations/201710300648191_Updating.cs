namespace DocToDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updating : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Subclasses", name: "Section_Id", newName: "Class_Id");
            RenameIndex(table: "dbo.Subclasses", name: "IX_Section_Id", newName: "IX_Class_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Subclasses", name: "IX_Class_Id", newName: "IX_Section_Id");
            RenameColumn(table: "dbo.Subclasses", name: "Class_Id", newName: "Section_Id");
        }
    }
}
