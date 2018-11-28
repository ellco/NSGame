using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NSFrame
{
    public class TCPConnector
    {
        private TCPData _tcpData;
        private System.Threading.Thread _connectThread;
        private TcpClient TcpClient { get; set; }

        public Action<TCPStatus> TCPStatusChangedAction { get; set; }

        private TCPStatus _tcpStatus = TCPStatus.Default;
        public TCPStatus TCPStatus
        {
            get
            {
                lock (this)
                {
                    return _tcpStatus;
                }

            }
            set
            {
                lock (this)
                {
                    if (_tcpStatus != value)
                    {
                        UnityEngine.Debug.Log(string.Format("【NetConnector】NotifyNetStateChange {0} to {1} @thread{2}", _tcpStatus.ToString(), value.ToString(), System.Threading.Thread.CurrentThread.ManagedThreadId));

                        _tcpStatus = value;

                        if(ThreadManager.Instance != null)
                        {
                            ThreadManager.Instance.MainThreadAction -= NotifyNetStateChange;
                            ThreadManager.Instance.MainThreadAction += NotifyNetStateChange;
                        }

                    }
                }
            }
        }

        public TCPConnector(TCPData tcpData)
        {
            _tcpData = tcpData;
        }

        public int Start()
        {
            _connectThread = ThreadManager.Instance.CreateThread(()=>Connect());
            _connectThread.Start();
            return 0;
        }

        public int Restart()
        {
            StopPreConnect();
            Start();
            return 0;
        }

        public int Stop()
        {
            StopPreConnect();
            return 0;
        }

        private void Connect()
        {
            lock(this)
            {
                try
                {
                    IPAddress[] ipAdds = Dns.GetHostAddresses(_tcpData.IP);

                    UnityEngine.Debug.Log(string.Format("StartConnect {0}:{1} host address:", _tcpData.IP, _tcpData.Port));

                    for (int i = 0; i < ipAdds.Length; i++)
                    {
                        UnityEngine.Debug.Log(ipAdds[i].ToString());
                    }

                    if (ipAdds.Length > 0)
                    {
                        IPAddress ipAdrress = ipAdds[0];
                        AddressFamily addressFamiliy = ipAdrress.AddressFamily;

                        TcpClient = new TcpClient(addressFamiliy);
                        TCPStatus = TCPStatus.Connecting;
                        TcpClient.BeginConnect(ipAdds, _tcpData.Port, OnTcpConnected, this);
                    }
                    else
                    {

                    }
                }
                catch
                {

                }
            }
        }

        private void OnTcpConnected(IAsyncResult asyncresult)
        {
            lock (this)
            {
                //UnityEngine.Debug.Log(string.Format("【NetConnector】OnTcpConnected @thread({0}) ", System.Threading.Thread.CurrentThread.ManagedThreadId));

                TCPConnector connector = asyncresult.AsyncState as TCPConnector;


                if (connector != null
                    && TCPStatus == TCPStatus.Connecting
                    && connector.TcpClient == this.TcpClient
                    && connector.TcpClient.Connected)
                {
                    TCPStatus = TCPStatus.Connected;
                }
                else
                {
                    UnityEngine.Debug.LogError(string.Format("【NetConnector】OnTcpConnected Error @thread({0}) !!!!!!!!!!!!!!", System.Threading.Thread.CurrentThread.ManagedThreadId));
                }
            }
        }

        private void NotifyNetStateChange()
        {
            if (TCPStatusChangedAction != null)
            {
                TCPStatusChangedAction(_tcpStatus);
            }
            else
            {
                UnityEngine.Debug.LogError("【NetConnector】_stateListener is null");
            }
        }

        private void StopPreConnect()
        {
            if (_connectThread != null)
            {
                UnityEngine.Debug.Log("【NetConnector】Stop connect @thread(" + System.Threading.Thread.CurrentThread.ManagedThreadId + ")");
                _connectThread.Abort();
                _connectThread = null;
            }

            if (TcpClient != null)
            {
                TcpClient.Close();
                TcpClient = null;
                TCPStatus = TCPStatus.Offline;
            }

        }

    }
}
