using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using sbWHSScan.ScanObjectModel;
using sbWHSScan.ScanObjectModel.Messages;

namespace sbWHSScan.Provider
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
    ConcurrencyMode = ConcurrencyMode.Multiple,
    UseSynchronizationContext = false)]
    public class ProviderService : IProvider
    {
        public ProviderService()
        {
            m_core = s_core;
            m_callback = OperationContext.Current.GetCallbackChannel<IProviderCallback>();
            OperationContext.Current.Channel.Closed += Channel_Closed;

            m_core.AddConnection(this);

        }

        private void Channel_Closed(object sender, EventArgs e)
        {
            m_core.Disconnect(this);
        }

        private ProviderCore m_core;
        static private ProviderCore s_core = new ProviderCore();
        private IProviderCallback m_callback;

        internal void ScannerListReceived(GetScannerListResponse message)
        {
                        try
            {
                m_callback.ScannerListReceived(message);
            }
            catch (CommunicationException)
            {
                // we don't really care...
            }
            catch (TimeoutException)
            {
                // we don't really care about this either.
            }
        }

        internal void ScanToEmailReceived(ScanToEmailResponse message)
        {
                        try
            {
                m_callback.ScanToEmailReceived(message);
            }
            catch (CommunicationException)
            {
                // we don't really care...
            }
            catch (TimeoutException)
            {
                // we don't really care about this either.
            }
        }

        public void SendScannerListRequest(GetScannerListRequest message)
        {
            m_core.SendScannerListResponse(message);
        }

        public void SendScanToEmailRequest(ScanToEmailRequest message)
        {
            m_core.SendScanToEmailResponse(message);
        }
    }
}
