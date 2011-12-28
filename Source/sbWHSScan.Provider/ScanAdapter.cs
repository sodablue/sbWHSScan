using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfSharp.Drawing;
using WIA;
using System.Runtime.InteropServices;
using System.IO;
using PdfSharp.Pdf;
using System.Drawing.Imaging;

namespace sbWHSScan.Provider
{
    class ScanAdapter
    {
        public static string JpegFormat = ImageFormat.Jpeg.Guid.ToString("B");
        private string _deviceId;
        private PdfDocument _doc;
        private int _pages;

        int scanType = 4; // 2 for greyscale, 4 for black & white, 1 for color
        int dpi = 300;
        bool adf = false;
        private double width = 8.5;
        private double height = 11.0;

        public ScanAdapter(string deviceId, string paperSize, string scanSource)
        {
            this._deviceId = deviceId;
            switch (paperSize)
            {
                case "Letter":
                    height = 11;
                    break;
                case "Legal":
                    height = 14;
                    break;
                default:
                    break;
            }

            this.adf = (scanSource == "Feeder");
            this._doc = new PdfDocument();
            _pages = 0;
        }

        public PdfDocument ScanToPDF()
        {
            ImageFile image;
            while ((image = this.ScanOne()) != null)
            {
                // save image to a temp file and then load into an XImage
                string tempPath = System.IO.Path.GetTempFileName();
                File.Delete(tempPath);
                tempPath = System.IO.Path.ChangeExtension(tempPath, "jpg");
                image.SaveFile(tempPath);
                XImage ximage = XImage.FromFile(tempPath);

                PdfPage page = this._doc.AddPage();
                _pages++;
                page.Width = XUnit.FromInch(this.width);
                page.Height = XUnit.FromInch(this.height);
                using (XGraphics xgraphics = XGraphics.FromPdfPage(page))
                {
                    xgraphics.DrawImage(ximage, 0, 0);
                    ximage.Dispose();
                    File.Delete(tempPath);
                }
                if (!this.adf)
                    break;
            }

            return _doc;
        }

        private bool SelectDevice()
        {
            try
            {
                // ISSUE: variable of a compiler-generated type
                WIA.CommonDialog commonDialog = (WIA.CommonDialog)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("850D1D11-70F3-4BE5-9A11-77AA6B2BB201")));
                // ISSUE: reference to a compiler-generated method
                // ISSUE: variable of a compiler-generated type
                Device device = commonDialog.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, false, true);
                this._deviceId = device.DeviceID;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to select scanner");
                Console.Error.WriteLine(ex.ToString());
            }
            return !string.IsNullOrEmpty(this._deviceId);
        }

        private ImageFile ScanOne()
        {
            ImageFile image = null;

            try
            {
                // find our device (scanner previously selected with commonDialog.ShowSelectDevice)
                DeviceManager manager = new DeviceManager();
                DeviceInfo deviceInfo = null;
                foreach (DeviceInfo info in manager.DeviceInfos)
                {
                    if (info.DeviceID == _deviceId)
                    {
                        deviceInfo = info;
                    }
                }

                if (deviceInfo != null)
                {
                    Device device = deviceInfo.Connect();

                    Item item = device.Items[1];

                    // configure item
                    SetItemIntProperty(ref item, 6146, this.scanType); // 2 for greyscale, 4 for black & white, 1 for color
                    if (this.scanType != 4) SetItemIntProperty(ref item, 4104, 8); // bit depth

                    SetItemIntProperty(ref item, 6147, this.dpi); // 150 dpi
                    SetItemIntProperty(ref item, 6148, this.dpi); // 150 dpi
                    SetItemIntProperty(ref item, 6151, (int)(this.dpi * this.width)); // scan width
                    SetItemIntProperty(ref item, 6152, (int)(this.dpi * this.height)); // scan height

                    int deviceHandling = this.adf ? 1 : 2; // 1 for ADF, 2 for flatbed

                    // configure device
                    SetDeviceIntProperty(ref device, 3088, deviceHandling);
                    int handlingStatus = GetDeviceIntProperty(ref device, 3087);

                    if (handlingStatus == deviceHandling)
                    {
                        image = item.Transfer(JpegFormat);

                    }
                }
            }
            catch (COMException ex)
            {
                image = null;

                // paper empty
                if ((uint)ex.ErrorCode != 0x80210003)
                {
                    throw;
                }
            }

            return image;
        }


        private void SetDeviceIntProperty(ref Device device, int propertyID, int propertyValue)
        {
            foreach (Property p in device.Properties)
            {
                if (p.PropertyID == propertyID)
                {
                    object value = propertyValue;
                    p.set_Value(ref value);
                    break;
                }
            }
        }

        private int GetDeviceIntProperty(ref Device device, int propertyID)
        {
            int ret = -1;

            foreach (Property p in device.Properties)
            {
                if (p.PropertyID == propertyID)
                {
                    ret = (int)p.get_Value();
                    break;
                }
            }

            return ret;
        }

        private void SetItemIntProperty(ref Item item, int propertyID, int propertyValue)
        {
            foreach (Property p in item.Properties)
            {
                if (p.PropertyID == propertyID)
                {
                    object value = propertyValue;
                    p.set_Value(ref value);
                    break;
                }
            }
        }

        private int GetItemIntProperty(ref Item item, int propertyID)
        {
            int ret = -1;

            foreach (Property p in item.Properties)
            {
                if (p.PropertyID == propertyID)
                {
                    ret = (int)p.get_Value();
                    break;
                }
            }

            return ret;
        }
    }
}
