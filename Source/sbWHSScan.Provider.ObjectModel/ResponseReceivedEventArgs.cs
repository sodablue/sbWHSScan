using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbWHSScan.Provider.ObjectModel.Messages;

namespace sbWHSScan.Provider.ObjectModel
{
    public class ResponseReceivedEventArgs : EventArgs
    {
        public ResponseMessageBase Message { get; set; }

        public ResponseReceivedEventArgs(ResponseMessageBase message)
        {
            this.Message = message;
        }
    }
}
