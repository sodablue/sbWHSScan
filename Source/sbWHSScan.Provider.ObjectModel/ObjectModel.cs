using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using sbWHSScan.ScanObjectModel.Messages;
using System.Collections.ObjectModel;

namespace sbWHSScan.ScanObjectModel
{
    public class ObjectModel : INotifyPropertyChanged
    {
        private bool m_connected;
        public ObjectModel()
        {
            //This Initialize method is needed to link to Windows Server references that are not GACed
            Microsoft.WindowsServerSolutions.Common.WindowsServerSolutionsEnvironment.Initialize();

            m_backend = new ObjectModelImplementation(this);
            this.PaperSizes.Add("Letter");
            this.PaperSizes.Add("Legal");
            this.PaperSize = "Letter";
            this.ScanSources.Add("Flatbed");
            this.ScanSources.Add("Feeder");
            this.ScanSource = "Flatbed";

            this.ScanDestinations.Add("Email");
            this.ScanDestinations.Add("Folder");
            this.ScanDestination = "Email";

            this.EmailProviders.Add("GMail");
            this.EmailProvider = "GMail";
            this.Connect();
        }

        public bool Connected
        {
            get { return m_connected; }
            internal set
            {
                if (m_connected != value)
                {
                    m_connected = value;
                    RaisePropertyChanged("Connected");
                }
            }
        }

        private readonly ObservableCollection<GetScannerListResponse.Scanner> scanners = new ObservableCollection<GetScannerListResponse.Scanner>();
        public ObservableCollection<GetScannerListResponse.Scanner> Scanners
        {
            get { return scanners; }
        }

        private GetScannerListResponse.Scanner scanner;
        public GetScannerListResponse.Scanner Scanner
        {
            get { return scanner; }
            set
            {
                if (scanner != value)
                {
                    scanner = value;
                    RaisePropertyChanged("Scanner");
                }
            }
        }

        private readonly ObservableCollection<string> papersizes = new ObservableCollection<string>();
        public ObservableCollection<string> PaperSizes
        {
            get { return papersizes; }
        }

        private string papersize;
        public string PaperSize
        {
            get { return papersize; }
            set
            {
                if (papersize != value)
                {
                    papersize = value;
                    RaisePropertyChanged("PaperSize");
                }
            }
        }

        private readonly ObservableCollection<string> scansources = new ObservableCollection<string>();
        public ObservableCollection<string> ScanSources
        {
            get { return scansources; }
        }

        private string scansource;
        public string ScanSource
        {
            get { return scansource; }
            set
            {
                if (scansource != value)
                {
                    scansource = value;
                    RaisePropertyChanged("ScanSource");
                }
            }
        }

        private readonly ObservableCollection<string> scandestinations = new ObservableCollection<string>();
        public ObservableCollection<string> ScanDestinations
        {
            get { return scandestinations; }
        }

        private string scandestination;
        public string ScanDestination
        {
            get { return scandestination; }
            set
            {
                if (scandestination != value)
                {
                    scandestination = value;
                    RaisePropertyChanged("ScanDestination");
                }
            }
        }

        private readonly ObservableCollection<string> emailproviders = new ObservableCollection<string>();
        public ObservableCollection<string> EmailProviders
        {
            get { return emailproviders; }
        }

        private string emailprovider;
        public string EmailProvider
        {
            get { return emailprovider; }
            set
            {
                if (emailprovider != value)
                {
                    emailprovider = value;
                    RaisePropertyChanged("EmailProvider");
                }
            }
        }

        private string emailaddress;
        public string EmailAddress
        {
            get { return emailaddress; }
            set
            {
                if (emailaddress != value)
                {
                    emailaddress = value;
                    RaisePropertyChanged("EmailAddress");
                }
            }
        }

        private string emailpassword;
        public string EmailPassword
        {
            get { return emailpassword; }
            set
            {
                if (emailpassword != value)
                {
                    emailpassword = value;
                    RaisePropertyChanged("EmailPassword");
                }
            }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    RaisePropertyChanged("Status");
                }
            }
        }


        private ObjectModelImplementation m_backend;

        private void Connect()
        {
            m_backend.Connect();
        }

        private void SendScannerListRequest(GetScannerListRequest message)
        {
            m_backend.SendScannerListRequest(message);
        }

        private void SendScanToEmailRequest(ScanToEmailRequest message)
        {
            m_backend.SendScanToEmailRequest(message);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null) changed(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void ReceiveResponse(ResponseMessageBase message)
        {
            if (message is GetScannerListResponse) GetScannerListResponseReceived(message as GetScannerListResponse);
            if (message is ScanToEmailResponse) ScanToEmailResponseReceived(message as ScanToEmailResponse);
        }

        internal void ConnectionCompleted()
        {
            Connected = true;
            this.SendScannerListRequest(new GetScannerListRequest());
        }

        internal void Disconnected()
        {
            Connected = false;
        }

        private void GetScannerListResponseReceived(GetScannerListResponse response)
        {
            this.Scanners.Clear();
            foreach (var item in response.ScannerList)
            {
                this.Scanners.Add(item);
            }

            if (response.ScannerList.Count > 0)
            {
                this.Scanner = response.ScannerList[0];
            }
        }

        public void Scan()
        {
            this.Status = string.Empty;
            this.SendScanToEmailRequest(new ScanToEmailRequest { DeviceId = this.Scanner.DeviceId, PaperSize = this.PaperSize, ScanSource = this.ScanSource, EmailProvider = this.EmailProvider, EmailAddress = this.EmailAddress, EmailPassword = this.EmailPassword });
        }

        private byte[] scannedPdf;
        public byte[] ScannedPdf
        {
            get { return scannedPdf; }
            set
            {
                if (scannedPdf != value)
                {
                    scannedPdf = value;
                    RaisePropertyChanged("ScannedPdf");
                }
            }
        }

        private void ScanToEmailResponseReceived(ScanToEmailResponse response)
        {
            this.Status = "Scan completed.";
        }
    }
}
