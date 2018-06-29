using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Tipstaff.Infrastructure.Repositories;
using TPLibrary.Logger;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;
using TPLibrary.GuidGenerator;
using TPLibrary.S3API;

namespace Tipstaff.Services.Infrastructure
{
    public class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            
            //REPOSITORIES
            container.Register(Component.For<IApplicantRepository>().ImplementedBy<ApplicantRepository>());
            container.Register(Component.For<IAttendanceNotesRepository>().ImplementedBy<AttendanceNotesRepository>().LifestylePerWebRequest());
            container.Register(Component.For<IChildRepository>().ImplementedBy<ChildRepository>());
            container.Register(Component.For<IContactsRepository>().ImplementedBy<ContactsRepository>());
            container.Register(Component.For<IDocumentsRepository>().ImplementedBy<DocumentsRepository>());
            container.Register(Component.For<IFAQRepository>().ImplementedBy<FAQRepository>());
            container.Register(Component.For<ISolicitorFirmRepository>().ImplementedBy<SolicitorFirmRepository>());
            container.Register(Component.For<ISolicitorRepository>().ImplementedBy<SolicitorRepository>());
            container.Register(Component.For<ITemplateRepository>().ImplementedBy<TemplateRepository>());
            container.Register(Component.For<ITipstaffRecordRepository>().ImplementedBy<TipstaffRecordRepository>());
            container.Register(Component.For<IDeletedTipstaffRecordRepository>().ImplementedBy<DeletedTipstaffRecordRepository>());
            container.Register(Component.For<ICaseReviewRepository>().ImplementedBy<CaseReviewRepository>());
            container.Register(Component.For<IAddressRepository>().ImplementedBy<AddressRepository>());
            container.Register(Component.For<IRespondentRepository>().ImplementedBy<RespondentRepository>());
            container.Register(Component.For<IAuditEventRepository>().ImplementedBy<AuditEventRepository>());
            container.Register(Component.For<IPoliceForcesRepository>().ImplementedBy<PoliceForcesRepository>());
            container.Register(Component.For<IUsersRepository>().ImplementedBy<UsersRepository>());
            container.Register(Component.For<ITipstaffPoliceForcesRepository>().ImplementedBy<TipstaffPoliceForcesRepository>());

            //MISC
            container.Register(Component.For<IGuidGenerator>().ImplementedBy<GuidGenerator>());

            container.Register(Component.For(typeof(IDynamoAPI<>)).ImplementedBy(typeof(DynamoAPI<>)));

            container.Register(Component.For<IS3API>().ImplementedBy<S3API>());

            container.Register(Component.For<ICloudWatchLogger>().ImplementedBy<CloudWatchLogger>());
            //END MISC
        }
    }
}
