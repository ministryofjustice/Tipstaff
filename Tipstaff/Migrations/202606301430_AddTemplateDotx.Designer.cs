namespace Tipstaff.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.2.0-61023")]
    public sealed partial class AddTemplateDotx : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddTemplateDotx));
        
        string IMigrationMetadata.Id
        {
            get { return "202606301430_AddTemplateDotx"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
