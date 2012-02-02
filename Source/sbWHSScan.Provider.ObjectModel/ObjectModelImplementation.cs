using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Microsoft.WindowsServerSolutions.Common.ProviderFramework;
using sbWHSScan.ScanObjectModel.Messages;

namespace sbWHSScan.ScanObjectModel
{
    public class ObjectModelImplementation : IProviderCallback
    {
        private ObjectModel m_receiver;
        private ProviderConnector<IProvider> m_connector;

        public ObjectModelImplementation(ObjectModel receiver)
        {
            m_receiver = receiver;
            m_connector = ConnectorFactory.GetServerConnector<IProvider>("provider", this);
            m_connector.ConnectionOpened += m_connector_ConnectionOpened;
            m_connector.ConnectionClosed += m_connector_ConnectionClosed;
        }

        private void m_connector_ConnectionClosed(object sender, ProviderConnectionClosedArgs<IProvider> e)
        {
            m_receiver.Disconnected();
        }

        public void Connect()
        {
            m_connector.Connect();
        }

        void m_connector_ConnectionOpened(object sender, ProviderConnectionOpenedArgs<IProvider> e)
        {
            try
            {
                m_receiver.ConnectionCompleted();
            }
            catch (CommunicationException)
            {
                // not a big deal, our closed event will do appropriate cleanup
            }
            catch (TimeoutException)
            {
                // not a big deal, our closed event will do appropriate cleanup
            }
        }

        public void SendScannerListRequest(GetScannerListRequest message)
        {
            try
            {
                m_connector.Connection.SendScannerListRequest(message);
            }
            catch (CommunicationException)
            {
                // we don't care, the disconnection will be handled by our handler
            }
            catch (TimeoutException)
            {
                // we don't really care here, although timeouts in a local system
                // generally represent an error case.
            }
        }

        public void SendScanToEmailRequest(ScanToEmailRequest message)
        {
            try
            {
                m_connector.Connection.SendScanToEmailRequest(message);
            }
            catch (CommunicationException)
            {
                // we don't care, the disconnection will be handled by our handler
            }
            catch (TimeoutException)
            {
                // we don't really care here, although timeouts in a local system
                // generally represent an error case.
            }
        }

        public void ScannerListReceived(GetScannerListResponse message)
        {
            m_receiver.ReceiveResponse(message);
        }

        public void ScanToEmailReceived(ScanToEmailResponse message)
        {
            m_receiver.ReceiveResponse(message);
        }
    }
}
