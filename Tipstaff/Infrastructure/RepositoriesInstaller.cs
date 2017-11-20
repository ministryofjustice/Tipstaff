using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Infrastructure.S3API;
using Tipstaff.Infrastructure.Services;
using Tipstaff.Logger;
using Tipstaff.Services.Repositories;

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

            //MISC
            container.Register(Component.For<IGuidGenerator>().ImplementedBy<GuidGenerator>());

            container.Register(Component.For(typeof(IDynamoAPI<>)).ImplementedBy(typeof(DynamoAPI<>)));

            container.Register(Component.For<IS3API>().ImplementedBy<S3API>());

            container.Register(Component.For<ICloudWatchLogger>().ImplementedBy<CloudWatchLogger>());
            //END MISC
        }
    }
}
