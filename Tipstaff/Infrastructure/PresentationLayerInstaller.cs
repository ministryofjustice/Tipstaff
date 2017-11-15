using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Web.Mvc;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Infrastructure.S3API;
using Tipstaff.Infrastructure.Services;
using Tipstaff.Logger;
using Tipstaff.Presenters;
using Tipstaff.Presenters.Interfaces;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Infrastructure
{
    public class PresentationLayerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //PRESENTERS
            container.Register(Component.For<IGraphPresenter>().ImplementedBy<GraphPresenter>());
            container.Register(Component.For<ISearchPresenter>().ImplementedBy<SearchPresenter>());
            container.Register(Component.For<IAddressPresenter>().ImplementedBy<AddressPresenter>());
            container.Register(Component.For<IApplicantPresenter>().ImplementedBy<ApplicantPresenter>());
            container.Register(Component.For<IAttendanceNotePresenter>().ImplementedBy<AttendanceNotePresenter>());
            container.Register(Component.For<ICaseReviewPresenter>().ImplementedBy<CaseReviewPresenter>());
            container.Register(Component.For<IChildAbductionPresenter>().ImplementedBy<ChildAbductionPresenter>());
            container.Register(Component.For<IChildPresenter>().ImplementedBy<ChildPresenter>());
            container.Register(Component.For<IPoliceForcesPresenter>().ImplementedBy<PoliceForcesPresenter>());
            container.Register(Component.For<IRespondentPresenter>().ImplementedBy<RespondentPresenter>());
            container.Register(Component.For<ISolicitorFirmsPresenter>().ImplementedBy<SolicitorFirmsPresenter>());
            container.Register(Component.For<ISolicitorPresenter>().ImplementedBy<SolicitorPresenter>());
            container.Register(Component.For<ITemplatePresenter>().ImplementedBy<TemplatePresenter>());
            container.Register(Component.For<ITipstaffRecordPresenter>().ImplementedBy<TipstaffRecordPresenter>());
            container.Register(Component.For<IFAQPresenter>().ImplementedBy<FAQPresenter>());
            //END PRESENTERS


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


            //MISC
            container.Register(Component.For<IGuidGenerator>().ImplementedBy<GuidGenerator>());

            container.Register(Component.For(typeof(IDynamoAPI<>)).ImplementedBy(typeof(DynamoAPI<>)));

            container.Register(Component.For<IS3API>().ImplementedBy<S3API.S3API>());

            container.Register(Component.For<ICloudWatchLogger>().ImplementedBy<CloudWatchLogger>());
            //END MISC
        }
    }
}
