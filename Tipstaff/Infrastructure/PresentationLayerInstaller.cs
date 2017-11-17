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
        }
    }
}
