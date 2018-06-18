namespace SuperSecret.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class endretDataAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Country", "CountryName", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Crime", "Description", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.TypeOfCrime", "CrimeTypeName", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TypeOfCrime", "CrimeTypeName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Crime", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Country", "CountryName", c => c.String(maxLength: 255));
        }
    }
}
