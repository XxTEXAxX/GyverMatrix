using System;
using GyverMatrix.Helpers;
using Xamarin.Forms.Xaml;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectPage {
        public ConnectPage() =>
            InitializeComponent();

        bool a = false;
        private void ConnectButton_OnClicked(object sender, EventArgs e)
        {
            UdpHelper.Connect(IpAdress.Text, int.Parse(Port.Text));
            a = true;
        }


        private void AutoConnectSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {

            if (a)
            {
                if (AutoConnectSwitch.IsToggled)
                {

                    UdpHelper.Send("$4 0 255;");

                }
                else
                {

                    UdpHelper.Send("$4 0 20;");

                }
            }

            
        }
    }
}