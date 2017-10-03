using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Infrastructure.S3API;
using Tipstaff.Logger;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Infrastructure
{
    public class PresentationLayerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IAttendanceNotesRepository>().ImplementedBy<AttendanceNotesRepository>().LifestylePerWebRequest());

            container.Register(Component.For<ITipstaffRecordRepository>().ImplementedBy<TipstaffRecordRepository>());

            container.Register(Component.For<IFAQRepository>().ImplementedBy<FAQRepository>());

            container.Register(Component.For<ISolicitorFirmRepository>().ImplementedBy<SolicitorFirmRepository>());

            container.Register(Component.For<IApplicantRepository>().ImplementedBy<ApplicantRepository>());

            container.Register(Component.For<IChildRepository>().ImplementedBy<ChildRepository>());

            container.Register(Component.For<IDocumentsRepository>().ImplementedBy<DocumentsRepository>());

            container.Register(Component.For<ITemplateRepository>().ImplementedBy<TemplateRepository>());

            container.Register(Component.For(typeof(IDynamoAPI<>))
                            .ImplementedBy(typeof(DynamoAPI<>)));

            container.Register(Component.For<IS3API>().ImplementedBy<S3API.S3API>());

            container.Register(Component.For<ICloudWatchLogger>().ImplementedBy<CloudWatchLogger>());
        }
    }
}
