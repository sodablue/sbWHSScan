using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbWHSScan.ScanObjectModel.Messages;
using sbWHSScan.Provider.Handlers;
using System.Reflection;

namespace sbWHSScan.Provider
{
    public class ProviderCore
    {
        private HashSet<ProviderService> m_connections = new HashSet<ProviderService>();
        private object m_syncRoot = new object();

        public void SendScannerListResponse(GetScannerListRequest message)
        {
            lock (m_syncRoot)
            {
                foreach (var connection in m_connections)
                {
                    GetScannerListHandler handler = new GetScannerListHandler();
                    
                    connection.ScannerListReceived(handler.Handle(message));
                }
            }
        }

        public void SendScanToEmailResponse(ScanToEmailRequest message)
        {
            lock (m_syncRoot)
            {
                foreach (var connection in m_connections)
                {
                    ScanToEmailHandler handler = new ScanToEmailHandler();

                    connection.ScanToEmailReceived(handler.Handle(message));
                }
            }
        }

        public void AddConnection(ProviderService connection)
        {
            lock (m_syncRoot)
            {
                m_connections.Add(connection);
            }

        }

        public void Disconnect(ProviderService connection)
        {
            lock (m_syncRoot)
            {
                m_connections.Remove(connection);
            }
        }
    }
}
