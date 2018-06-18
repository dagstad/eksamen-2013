namespace SuperSecret.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testetUtenSuspectIdCrimes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Crime", "Suspect_SuspectId", "dbo.Suspect");
            DropForeignKey("dbo.Suspect", "Crime_CrimeId", "dbo.Crime");
            DropForeignKey("dbo.Crime", "Suspect_SuspectId1", "dbo.Suspect");
            DropIndex("dbo.Suspect", new[] { "Crime_CrimeId" });
            DropIndex("dbo.Crime", new[] { "Suspect_SuspectId" });
            DropIndex("dbo.Crime", new[] { "Suspect_SuspectId1" });
            CreateTable(
                "dbo.CrimeSuspect",
                c => new
                    {
                        Crime_CrimeId = c.Int(nullable: false),
                        Suspect_SuspectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Crime_CrimeId, t.Suspect_SuspectId })
                .ForeignKey("dbo.Crime", t => t.Crime_CrimeId, cascadeDelete: true)
                .ForeignKey("dbo.Suspect", t => t.Suspect_SuspectId, cascadeDelete: true)
                .Index(t => t.Crime_CrimeId)
                .Index(t => t.Suspect_SuspectId);
            
            DropColumn("dbo.Suspect", "Crime_CrimeId");
            DropColumn("dbo.Crime", "SuspectId");
            DropColumn("dbo.Crime", "Suspect_SuspectId");
            DropColumn("dbo.Crime", "Suspect_SuspectId1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Crime", "Suspect_SuspectId1", c => c.Int());
            AddColumn("dbo.Crime", "Suspect_SuspectId", c => c.Int());
            AddColumn("dbo.Crime", "SuspectId", c => c.Int());
            AddColumn("dbo.Suspect", "Crime_CrimeId", c => c.Int());
            DropForeignKey("dbo.CrimeSuspect", "Suspect_SuspectId", "dbo.Suspect");
            DropForeignKey("dbo.CrimeSuspect", "Crime_CrimeId", "dbo.Crime");
            DropIndex("dbo.CrimeSuspect", new[] { "Suspect_SuspectId" });
            DropIndex("dbo.CrimeSuspect", new[] { "Crime_CrimeId" });
            DropTable("dbo.CrimeSuspect");
            CreateIndex("dbo.Crime", "Suspect_SuspectId1");
            CreateIndex("dbo.Crime", "Suspect_SuspectId");
            CreateIndex("dbo.Suspect", "Crime_CrimeId");
            AddForeignKey("dbo.Crime", "Suspect_SuspectId1", "dbo.Suspect", "SuspectId");
            AddForeignKey("dbo.Suspect", "Crime_CrimeId", "dbo.Crime", "CrimeId");
            AddForeignKey("dbo.Crime", "Suspect_SuspectId", "dbo.Suspect", "SuspectId");
        }
    }
}
