using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace sbWHSScan.ScanObjectModel.Messages
{
    public class GetScannerListRequest : RequestMessageBase
    {
    }

    public class GetScannerListResponse : ResponseMessageBase
    {
        public class Scanner
        {
            public string DeviceId { get; set; }
            public string DeviceName { get; set; }
        }

        public List<Scanner> ScannerList { get; set; }
    }

}
