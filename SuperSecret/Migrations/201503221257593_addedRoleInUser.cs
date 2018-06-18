namespace SuperSecret.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedRoleInUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Role", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "Role");
        }
    }
}
