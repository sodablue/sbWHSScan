using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbWHSScan.Provider.ObjectModel.Messages;
using WIA;

namespace sbWHSScan.Provider.Handlers
{
    public class GetScannerListHandler : IMessageHandler<GetScannerListRequest>
    {
        public ResponseMessageBase Handle(GetScannerListRequest message)
        {
            GetScannerListResponse response = new GetScannerListResponse();
            response.ScannerList = GetScanners().ToList();

            return response;
        }

        public ResponseMessageBase Handle(RequestMessageBase message)
        {
            return this.Handle(message as GetScannerListRequest);
        }

        private IEnumerable<GetScannerListResponse.Scanner> GetScanners()
        {
            DeviceManager manager = new DeviceManager();
            foreach (DeviceInfo info in manager.DeviceInfos)
            {
                if (info.Type == WiaDeviceType.ScannerDeviceType)
                {
                    string deviceId = info.DeviceID;
                    string deviceName = null;
                    foreach (Property item in info.Properties)
                    {
                        if (item.Name == "Description") 
                            deviceName = item.get_Value();
                    }

                    if (deviceName != null)
                        yield return new GetScannerListResponse.Scanner() {DeviceId = deviceId, DeviceName = deviceName};
                }
            }
        }
    }
}
