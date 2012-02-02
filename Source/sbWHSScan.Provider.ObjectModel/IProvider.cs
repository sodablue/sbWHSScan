using System.ServiceModel;
using Microsoft.WindowsServerSolutions.Common.ProviderFramework;
using sbWHSScan.ScanObjectModel.Messages;

namespace sbWHSScan.ScanObjectModel
{
    [ProviderEndpointBehavior(CredentialType.User,ConnectionSetting.AllowRemoteAccess)]
    [ServiceContract(CallbackContract = typeof(IProviderCallback))]
    public interface IProvider
    {
        [OperationContract(IsOneWay = true)]
        void SendScannerListRequest(GetScannerListRequest message);

        [OperationContract(IsOneWay = true)]
        void SendScanToEmailRequest(ScanToEmailRequest message);
    }
}