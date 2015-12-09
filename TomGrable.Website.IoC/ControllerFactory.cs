using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Helpers;
using Sitecore.StringExtensions;

namespace TomGrable.Website.IoC
{
    /// <summary>
    /// This ControllerFactory creates MVC controllers and uses SimpleInjector 
    /// to inject dependencies.
    /// </summary>
    public class ControllerFactory : DefaultControllerFactory
    {
        private IControllerFactory innerFactory;

        public ControllerFactory(IControllerFactory innerFactory)
        {
            this.innerFactory = innerFactory;
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            Type controllerType = null;

            // If Sitecore thinks this looks like a type it can construct...
            if (TypeHelper.LooksLikeTypeName(controllerName))
            {
                // Get the type name.
                controllerType = TypeHelper.GetType(controllerName);
            }

            // If we still don't know the type...
            if (controllerType == null)
            {
                // Let the default controller factory resolve it.
                controllerType = this.GetControllerType(requestContext, controllerName);
            }

            // Now, if we know the type...
            if (controllerType != null)
            {
                // Try to get one from the IoC container.
                object controller = null;
                try { controller = Container.Current.GetInstance(controllerType); }
                catch (SimpleInjector.ActivationException x) { Log.Warn("Unable to get an instance of type from the IoC container: {0}".FormatWith(controllerType.ToString()), x, this); }                
                if (controller != null) return (IController)controller;
            }

            // If we're still here, delegate to the inner factory.
            return innerFactory.CreateController(requestContext, controllerName);
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return innerFactory.GetControllerSessionBehavior(requestContext, controllerName);
        }

        public override void ReleaseController(IController controller)
        {
            this.innerFactory.ReleaseController(controller);
        }
    }
}
