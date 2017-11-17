using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hospital.Implementation;

namespace Hospital.App_Start
{
    public class NinjectConfigurator
    {
        /// <summary>
        ///     Entry method used by caller to configure the given
        ///     container with all of this application's
        ///     dependencies.
        /// </summary>
        public void Configure(IKernel container)
        {
            AddBindings(container);
        }

        private void AddBindings(IKernel container)
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////
            // IMPORTANT NOTE! *Never* configure a type as singleton if it depends upon ISession!!! This is because
            // ISession is disposed of at the end of every request. For the same reason, make sure that types
            // dependent upon such types aren't configured as singleton.
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////

            container.Bind<IHospitalResource>().To<HospitalResource>().InSingletonScope();
        }

    }
}