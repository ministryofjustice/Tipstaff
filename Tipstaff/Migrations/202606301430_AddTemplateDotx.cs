namespace Tipstaff.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTemplateDotx : DbMigration
    {
        public override void Up()
        {
            // Make templateXML nullable
            AlterColumn("dbo.Templates", "templateXML", c => c.String());

            // Add the new required byte[] column
            AddColumn("dbo.Templates", "templateDOTX", c => c.Binary());
        }

        public override void Down()
        {
            // Remove the new column
            DropColumn("dbo.Templates", "templateDOTX");

            // Restore templateXML to required
            AlterColumn("dbo.Templates", "templateXML", c => c.String(nullable: false));
        }
    }
}
