using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EKContent.web.Models.Database.Abstract;
using EKContent.web.Models.Database.Concrete;

namespace EKContent.web.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {

        Ninject.IKernel _kernel = null;
        public NinjectControllerFactory()
        {
            _kernel = new Ninject.StandardKernel();
            _kernel.Bind<INavigationProvider>().To<NavigationProvider>();
            _kernel.Bind<IEkDataProvider>().To<EkDataProvider>();
            _kernel.Bind<IEkSiteDataProvider>().To<EkSiteDataProvider>();
            _kernel.Bind<IImageDataProvider>().To<ImageProvider>();
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return null;
            return (IController)_kernel.GetService(controllerType);
        }

    }
}