using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSFrame
{
    
    public class NetService
    {
        public static TCPManager TCPManager { get; set; }

        public static int StartTCP(TCPData tcpData)
        {
            if(TCPManager == null)
            {
                TCPManager = new TCPManager(tcpData);
                TCPManager.Start();
            }
            return 0;
        }

        public static int StopTCP()
        {
            if (TCPManager != null)
            {
                TCPManager.Stop();
            }
            return 0;
        }

    }

    
}
