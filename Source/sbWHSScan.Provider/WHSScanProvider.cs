using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsServerSolutions.Common.ProviderFramework;

namespace sbWHSScan.Provider
{
    public class WHSScanProvider
    {
        private ProviderHost host;

        public WHSScanProvider()
        {
            //This Initialize method is needed to link to Windows Server references that are not GACed
            Microsoft.WindowsServerSolutions.Common.WindowsServerSolutionsEnvironment.Initialize();
        }

        public void Start()
        {

            this.host = new ProviderHost(typeof(ProviderService), "provider");

            this.host.Open();

        }

        public void Stop()
        {
            this.host.Dispose();
        }
    }
}
