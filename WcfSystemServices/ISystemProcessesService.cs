using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfSystemServices
{
    public interface ISystemProcessesService
    {
        [OperationContract]
        string GetAllProcesses();

    // TODO: Add your service operations here
}

public class ProcessDetail
{
    public string processName { get; set; }
    public string owner { get; set; }
    public string fileName { get; set; }
}
    
}
