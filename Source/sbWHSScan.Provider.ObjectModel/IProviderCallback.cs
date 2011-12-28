using System.ServiceModel;
using sbWHSScan.Provider.ObjectModel.Messages;

namespace sbWHSScan.Provider.ObjectModel
{
    [ServiceContract]
    public interface IProviderCallback
    {
        [OperationContract(IsOneWay = true)]
        void ResponseReceived(ResponseMessageBase response);
    }
}