using System.ServiceModel;
using sbWHSScan.ScanObjectModel.Messages;

namespace sbWHSScan.ScanObjectModel
{
    [ServiceContract]
    public interface IProviderCallback
    {
        [OperationContract(IsOneWay = true)]
        void ScannerListReceived(GetScannerListResponse message);

        [OperationContract(IsOneWay = true)]
        void ScanToEmailReceived(ScanToEmailResponse message);
    }
}