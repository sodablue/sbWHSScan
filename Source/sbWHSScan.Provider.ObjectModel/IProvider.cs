using System.ServiceModel;
using Microsoft.WindowsServerSolutions.Common.ProviderFramework;
using sbWHSScan.Provider.ObjectModel.Messages;

namespace sbWHSScan.Provider.ObjectModel
{
    [ProviderEndpointBehavior(CredentialType.User)]
    [ServiceContract(CallbackContract = typeof(IProviderCallback))]
    public interface IProvider
    {
        [OperationContract(IsOneWay = true)]
        void SendRequest(RequestMessageBase message);
    }
}