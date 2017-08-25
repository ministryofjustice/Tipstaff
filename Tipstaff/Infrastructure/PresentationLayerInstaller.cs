using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Tipstaff.Infrastructure
{
    public class PresentationLayerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
           // container.Register(Component.For<ITelemetryLogger>().ImplementedBy<TelemetryLogger>().LifestyleSingleton());
        }
    }
}