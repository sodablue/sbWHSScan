using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfSharp.Pdf;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace sbWHSScan.Provider.Email
{
    class GMailProvider
    {
        public string EmailAddress { get; set; }
        public string EmailPassword { get; set; }
        public PdfDocument PdfDoc { get; set; }

        public void Execute()
        {
            using (MemoryStream attachment = new MemoryStream())
            {
                this.PdfDoc.Save(attachment, false);
                attachment.Position = 0;

                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(this.EmailAddress, this.EmailPassword);
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.Timeout = 60000;

                MailMessage mm = new MailMessage(this.EmailAddress, this.EmailAddress);
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mm.Attachments.Add(new Attachment(attachment, "untitled.pdf", System.Net.Mime.MediaTypeNames.Application.Pdf));
                client.Send(mm);
            }
        }
    }
}
