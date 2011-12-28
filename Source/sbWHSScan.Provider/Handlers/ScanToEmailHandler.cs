using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbWHSScan.Provider.ObjectModel.Messages;
using System.Net;
using System.Net.Mail;
using PdfSharp.Pdf;
using System.IO;
using sbWHSScan.Provider.Email;

namespace sbWHSScan.Provider.Handlers
{
    public class ScanToEmailHandler : IMessageHandler<ScanToEmailRequest>
    {
        public ResponseMessageBase Handle(ScanToEmailRequest message)
        {
            ScanToEmailResponse response = new ScanToEmailResponse();

            string tempPath = System.IO.Path.GetTempPath();


            ScanAdapter adapter = new ScanAdapter(message.DeviceId, message.PaperSize, message.ScanSource);
            PdfDocument pdfDoc = adapter.ScanToPDF();

            GMailProvider emailProvider = new GMailProvider();
            emailProvider.EmailAddress = message.EmailAddress;
            emailProvider.EmailPassword = message.EmailPassword;
            emailProvider.PdfDoc = pdfDoc;

            emailProvider.Execute();

            return response;
        }

        public ResponseMessageBase Handle(RequestMessageBase message)
        {
            return this.Handle(message as ScanToEmailRequest);
        }
    }
}
