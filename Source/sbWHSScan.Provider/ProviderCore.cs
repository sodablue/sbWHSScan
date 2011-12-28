using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbWHSScan.Provider.ObjectModel.Messages;
using sbWHSScan.Provider.Handlers;
using System.Reflection;

namespace sbWHSScan.Provider
{
    public class ProviderCore
    {
        private HashSet<ProviderService> m_connections = new HashSet<ProviderService>();
        private object m_syncRoot = new object();

        public void SendOperation(RequestMessageBase message)
        {
            lock (m_syncRoot)
            {
                foreach (var connection in m_connections)
                {
                    connection.SendToClient(RequestProcessor.Handle(message));
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
