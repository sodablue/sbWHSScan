using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using sbWHSScan.Provider.ObjectModel;
using sbWHSScan.Provider.ObjectModel.Messages;

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


        public void SendRequest(RequestMessageBase message)
        {
            m_core.SendOperation(message);
        }

        internal void SendToClient(ResponseMessageBase message)
        {
            try
            {
                m_callback.ResponseReceived(message);
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
    }
}
