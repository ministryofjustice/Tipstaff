namespace Tipstaff.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ContactTableRetentionColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "Retention", c => c.Boolean(nullable: true, defaultValue: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Contacts", "Retention");
        }
    }
}