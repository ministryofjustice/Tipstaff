namespace Tipstaff.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class TipstaffTableRetentionColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TipstaffRecords", "Retention", c => c.Boolean(nullable: true, defaultValue: false));
        }

        public override void Down()
        {
            DropColumn("dbo.TipstaffRecords", "Retention");
        }
    }
}