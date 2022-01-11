using System;
using GyverMatrix.Helpers;
using Xamarin.Forms.Xaml;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectPage {
        public ConnectPage() =>
            InitializeComponent();

        private async void ConnectButton_OnClicked(object sender, EventArgs e) {
            using (var udpHelper = new UdpHelper()) {
                udpHelper.Connect("3223", 34);
                await udpHelper.Send("");
            }
        }

        private void AutoConnectSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {

        }
    }
}