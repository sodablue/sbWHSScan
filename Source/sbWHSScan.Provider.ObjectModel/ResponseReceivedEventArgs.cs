using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbWHSScan.ScanObjectModel.Messages;

namespace sbWHSScan.ScanObjectModel
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
