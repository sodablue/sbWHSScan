using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbWHSScan.ScanObjectModel.Messages;
using System.Net;
using System.Net.Mail;
using PdfSharp.Pdf;
using System.IO;
using sbWHSScan.Provider.Email;
using log4net;

namespace sbWHSScan.Provider.Handlers
{
    public class ScanToEmailHandler
    {
        private ILog logger = log4net.LogManager.GetLogger(typeof(ScanToEmailHandler).FullName);

        public ScanToEmailResponse Handle(ScanToEmailRequest message)
        {
            ScanToEmailResponse response = new ScanToEmailResponse();

            string tempPath = System.IO.Path.GetTempPath();


            ScanAdapter adapter = new ScanAdapter(message.DeviceId, message.PaperSize, message.ScanSource);
            PdfDocument pdfDoc = adapter.ScanToPDF();

            logger.Debug(string.Format("Address: {0}  Password: {1}", message.EmailAddress, message.EmailPassword));
            GMailProvider emailProvider = new GMailProvider();
            emailProvider.EmailAddress = message.EmailAddress;
            emailProvider.EmailPassword = message.EmailPassword;
            emailProvider.PdfDoc = pdfDoc;

            emailProvider.Execute();

            return response;
        }
    }
}
