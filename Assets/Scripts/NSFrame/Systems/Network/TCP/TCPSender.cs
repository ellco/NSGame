using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSFrame
{
    public class TCPSender
    {
        private TCPConnector _connector;
        public TCPSender(TCPConnector connector)
        {
            _connector = connector;
            _connector.TCPStatusChangedAction += OnTCPStatusChanged;
        }

        private void OnTCPStatusChanged(TCPStatus tcpStatus)
        {

        }
    }
}
