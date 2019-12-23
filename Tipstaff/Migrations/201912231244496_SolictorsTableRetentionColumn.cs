namespace Tipstaff.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SolictorsTableRetentionColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Solicitors", "Retention", c => c.Boolean(nullable: true, defaultValue: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Solicitors", "Retention");
        }
    }
}