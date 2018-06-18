namespace SuperSecret.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExludedPictureClass : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Picture", "SuspectId", "dbo.Suspect");
            DropIndex("dbo.Picture", new[] { "SuspectId" });
            DropTable("dbo.Picture");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Picture",
                c => new
                    {
                        PictureId = c.Int(nullable: false, identity: true),
                        SuspectId = c.Int(nullable: false),
                        FileName = c.String(maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PictureId);
            
            CreateIndex("dbo.Picture", "SuspectId");
            AddForeignKey("dbo.Picture", "SuspectId", "dbo.Suspect", "SuspectId", cascadeDelete: true);
        }
    }
}
