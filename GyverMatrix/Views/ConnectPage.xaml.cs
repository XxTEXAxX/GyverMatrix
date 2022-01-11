using System;
using GyverMatrix.Helpers;
using Xamarin.Forms.Xaml;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectPage {
        public ConnectPage() =>
            InitializeComponent();

<<<<<<< HEAD
        private void ConnectButton_OnClicked(object sender, EventArgs e)
        {
            //using (var udpHelper = new UdpHelper())
            //{

                //udpHelper.Connect(IpAdress.Text,int.Parse(Port.Text));
            //}
=======
        private async void ConnectButton_OnClicked(object sender, EventArgs e) {
            using (var udpHelper = new UdpHelper()) {
                udpHelper.Connect("3223", 34);
                await udpHelper.Send("");
            }
>>>>>>> 3ea1919df69cff8c103df92a996af5af0362ea07
        }

        private void AutoConnectSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {

            if(AutoConnectSwitch.IsToggled)
            {
                using (var udpHelper = new UdpHelper())
                {
                    udpHelper.Connect(IpAdress.Text, int.Parse(Port.Text));
                    udpHelper.Send("$4 0 255;");
                }
            }else{
                using (var udpHelper = new UdpHelper())
                {
                    udpHelper.Connect(IpAdress.Text, int.Parse(Port.Text));
                    udpHelper.Send("$4 0 20;");
                }
            }

        }
    }
}