using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSFrame
{
    public class TCPReceiver
    {
        private TCPConnector _connector;
        public TCPReceiver(TCPConnector connector)
        {
            _connector = connector;
            _connector.TCPStatusChangedAction += OnTCPStatusChanged;
        }

        private void OnTCPStatusChanged(TCPStatus tcpStatus)
        {

        }
    }
}
