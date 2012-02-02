using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsServerSolutions.Common.ProviderFramework;
using log4net.Config;
using System.IO;
using Topshelf;

namespace sbWHSScan.Provider
{
    public class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.ConfigureAndWatch(
                new FileInfo(".\\log4net.config"));

            var host = HostFactory.New(x =>
            {
                x.EnableDashboard();
                x.Service<WHSScanProvider>(s =>
                {
                    s.SetServiceName("WHSScanProvider");
                    s.ConstructUsing(name => new WHSScanProvider());
                    s.WhenStarted(tc =>
                    {
                        XmlConfigurator.ConfigureAndWatch(
                            new FileInfo(".\\log4net.config"));
                        tc.Start();
                    });
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();
                x.SetDescription("WHSScanProvider Description");
                x.SetDisplayName("WHSScanProvider");
                x.SetServiceName("WHSScanProvider");
            });

            host.Run();
        }
    }
}
