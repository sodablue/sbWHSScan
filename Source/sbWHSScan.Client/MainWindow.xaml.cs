using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using sbWHSScan.Provider.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using sbWHSScan.Provider.ObjectModel.Messages;

namespace sbWHSScan.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            if (this.DataContext == null) this.DataContext = new ObjectModel();
            ObjectModel objModel = this.DataContext as ObjectModel;
            objModel.PropertyChanged += objModel_PropertyChanged;
        }


        void objModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Connected")
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal,
                   new Action(() => this.ConnectedChanged()));
            }
        }

        private void ConnectedChanged()
        {
            ObjectModel objModel = this.DataContext as ObjectModel;
            bool connected = objModel.Connected;

            if (connected)
            {
                this.SelectScannerPanel.IsEnabled = true;
            }
            else
            {
                this.SelectScannerPanel.IsEnabled = false;
            }
        }

        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext == null) this.DataContext = new ObjectModel();
            ObjectModel objModel = this.DataContext as ObjectModel;

            objModel.Scan();
        }
    }

}
