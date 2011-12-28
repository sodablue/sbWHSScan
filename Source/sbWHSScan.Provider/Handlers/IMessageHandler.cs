using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbWHSScan.Provider.ObjectModel.Messages;

namespace sbWHSScan.Provider.Handlers
{
    public interface IMessageHandler
    {
        ResponseMessageBase Handle(RequestMessageBase message);
    }

    public interface IMessageHandler<TRequest> : IMessageHandler
        where TRequest : RequestMessageBase
    {
        ResponseMessageBase Handle(TRequest message);
    }
}
