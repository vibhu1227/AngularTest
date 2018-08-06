using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;

namespace SystemServicesInfo
{
    public partial class SystemInfoService : ServiceBase
    {
        ServiceHost host;
        public SystemInfoService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
           
            host = new ServiceHost(typeof(WcfSystemServices.SystemProcessesService));
            host.Open();
        }

        protected override void OnStop()
        {
            host.Close();
        }
    }
}
