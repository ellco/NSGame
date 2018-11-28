using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSFrame
{
    public class TCPManager
    {
        public TCPData TCPData { get; set; }
        public TCPConnector TCPConnector { get; set; }
        public TCPSender TCPSender { get; set; }
        public TCPReceiver TCPReceiver { get; set; }
        public TCPManager(TCPData tcpData)
        {
            TCPData = tcpData;
        }

        public void Start()
        {
            TCPConnector = new TCPConnector(TCPData);
            TCPConnector.Start();

            TCPSender = new TCPSender(TCPConnector);
            TCPReceiver = new TCPReceiver(TCPConnector);
        }

        public void Stop()
        {
            TCPConnector.Stop();
        }

        
    }

}
