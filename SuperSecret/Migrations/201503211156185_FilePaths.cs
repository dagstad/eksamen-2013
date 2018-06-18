namespace SuperSecret.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FilePaths : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FilePath",
                c => new
                    {
                        FilePathId = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        Filetype = c.Int(nullable: false),
                        SuspectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FilePathId)
                .ForeignKey("dbo.Suspect", t => t.SuspectId, cascadeDelete: true)
                .Index(t => t.SuspectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FilePath", "SuspectId", "dbo.Suspect");
            DropIndex("dbo.FilePath", new[] { "SuspectId" });
            DropTable("dbo.FilePath");
        }
    }
}
