namespace SuperSecret.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialcreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        CountryName = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "dbo.Suspect",
                c => new
                    {
                        SuspectId = c.Int(nullable: false, identity: true),
                        CountryId = c.Int(),
                        PictureId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 50),
                        Alias = c.String(nullable: false, maxLength: 50),
                        Age = c.Int(nullable: false),
                        Crime_CrimeId = c.Int(),
                    })
                .PrimaryKey(t => t.SuspectId)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .ForeignKey("dbo.Crime", t => t.Crime_CrimeId)
                .Index(t => t.CountryId)
                .Index(t => t.Crime_CrimeId);
            
            CreateTable(
                "dbo.Crime",
                c => new
                    {
                        CrimeId = c.Int(nullable: false, identity: true),
                        SuspectId = c.Int(),
                        TypeOfCrimeId = c.Int(nullable: false),
                        Description = c.String(maxLength: 1000),
                        Date = c.DateTime(nullable: false),
                        Suspect_SuspectId = c.Int(),
                        Suspect_SuspectId1 = c.Int(),
                    })
                .PrimaryKey(t => t.CrimeId)
                .ForeignKey("dbo.Suspect", t => t.Suspect_SuspectId)
                .ForeignKey("dbo.TypeOfCrime", t => t.TypeOfCrimeId, cascadeDelete: true)
                .ForeignKey("dbo.Suspect", t => t.Suspect_SuspectId1)
                .Index(t => t.TypeOfCrimeId)
                .Index(t => t.Suspect_SuspectId)
                .Index(t => t.Suspect_SuspectId1);
            
            CreateTable(
                "dbo.TypeOfCrime",
                c => new
                    {
                        TypeOfCrimeId = c.Int(nullable: false, identity: true),
                        CrimeTypeName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.TypeOfCrimeId);
            
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
                .PrimaryKey(t => t.PictureId)
                .ForeignKey("dbo.Suspect", t => t.SuspectId, cascadeDelete: true)
                .Index(t => t.SuspectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Picture", "SuspectId", "dbo.Suspect");
            DropForeignKey("dbo.Crime", "Suspect_SuspectId1", "dbo.Suspect");
            DropForeignKey("dbo.Crime", "TypeOfCrimeId", "dbo.TypeOfCrime");
            DropForeignKey("dbo.Suspect", "Crime_CrimeId", "dbo.Crime");
            DropForeignKey("dbo.Crime", "Suspect_SuspectId", "dbo.Suspect");
            DropForeignKey("dbo.Suspect", "CountryId", "dbo.Country");
            DropIndex("dbo.Picture", new[] { "SuspectId" });
            DropIndex("dbo.Crime", new[] { "Suspect_SuspectId1" });
            DropIndex("dbo.Crime", new[] { "Suspect_SuspectId" });
            DropIndex("dbo.Crime", new[] { "TypeOfCrimeId" });
            DropIndex("dbo.Suspect", new[] { "Crime_CrimeId" });
            DropIndex("dbo.Suspect", new[] { "CountryId" });
            DropTable("dbo.Picture");
            DropTable("dbo.TypeOfCrime");
            DropTable("dbo.Crime");
            DropTable("dbo.Suspect");
            DropTable("dbo.Country");
        }
    }
}
