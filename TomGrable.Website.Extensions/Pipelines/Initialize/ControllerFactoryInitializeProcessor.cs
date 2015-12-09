using System;
using System.Linq;
using System.Web.Mvc;
using SimpleInjector;
using Sitecore.Configuration;
using Sitecore.Pipelines;
using Sitecore.Text;
using TomGrable.Website.IoC;
using Container = TomGrable.Website.IoC.Container;

namespace TomGrable.Website.Extensions.Pipelines.Initialize
{
    public class ControllerFactoryInitializeProcessor
    {
        public void Process(PipelineArgs args)
        {
            //todo: clean this code
            //auto bind all services
            AutoMap();

            // Create our controller factory - pass in the default factory.
            var factory = new ControllerFactory(ControllerBuilder.Current.GetControllerFactory());
            // Tell MVC to use our controller factory.
            ControllerBuilder.Current.SetControllerFactory(factory);
        }


        private void AutoMap()
        {
            var assembliesToRegister = new ListString(Settings.GetSetting("SimpleInjectorAssembliesToRegister", "TomGrable.Website.Core"));
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => assembliesToRegister.Any(assembly => a.FullName.StartsWith(assembly)));
            var registrations =
                assemblies.SelectMany(assembly => assembly.GetExportedTypes().Where(type => type.GetInterfaces().Any())).Select(type => new
                {
                    Service = type.GetInterfaces().Single(),
                    Implementation = type
                });
            foreach (var reg in registrations)
            {
                Container.Current.Register(reg.Service, reg.Implementation, Lifestyle.Transient);
            }
        }
    }

}
