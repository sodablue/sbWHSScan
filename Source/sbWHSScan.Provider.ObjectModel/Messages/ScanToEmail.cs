using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sbWHSScan.ScanObjectModel.Messages
{
    public class ScanToEmailRequest : RequestMessageBase
    {
        public string DeviceId { get; set; }
        public string PaperSize { get; set; }
        public string ScanSource { get; set; }
        public string EmailProvider { get; set; }
        public string EmailAddress { get; set; }
        public string EmailPassword { get; set; }
    }

    public class ScanToEmailResponse : ResponseMessageBase
    {
    }

}
