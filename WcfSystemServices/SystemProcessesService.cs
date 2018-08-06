using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;

namespace WcfSystemServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SystemProcessesService" in both code and config file together.
    public class SystemProcessesService : ISystemProcessesService
    {
        internal static IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

        //the User Name is the info we want returned by the query.
        internal static int WTS_UserName = 5;
        public string GetAllProcesses()
        {
            //  string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            Process[] ipByName = Process.GetProcesses();
            List<ProcessDetail> names = new List<ProcessDetail>();
            ProcessDetail pd = null;
            foreach (Process proc in ipByName)
            {
                pd = new ProcessDetail();
                pd.processName = proc.ProcessName;
                pd.fileName = proc.MainWindowTitle;
                //access to the Idle process is restricted, so don't try to access it.
                if (proc.ProcessName != "Idle")
                {
                    IntPtr AnswerBytes;
                    IntPtr AnswerCount;
                    if (WTSQuerySessionInformationW(WTS_CURRENT_SERVER_HANDLE,
                                                    proc.SessionId,
                                                    WTS_UserName,
                                                    out AnswerBytes,
                                                    out AnswerCount))
                    {
                        string userName = Marshal.PtrToStringUni(AnswerBytes);
                        pd.owner = userName;

                    }
                    else
                    {
                        Console.WriteLine("Could not access Terminal Services Server.");
                    }
                }
                if (pd.owner != "")
                {
                    names.Add(pd);
                }

            }
            var xEle = new XElement("Processes",
                from proc in names
                select new XElement("ProcessDetail",
                               new XElement("processName", proc.processName),
                               new XElement("fileName", proc.fileName),
                               new XElement("owner", proc.owner)


                           ));
            //  return Serialize<List<ProcessDetail>>(names);
            return xEle.ToString();

        }


        [DllImport("Wtsapi32.dll")]
        public static extern bool WTSQuerySessionInformationW(
      IntPtr hServer,
      int SessionId,
      int WTSInfoClass,
      out IntPtr ppBuffer,
      out IntPtr pBytesReturned);
    }
}
