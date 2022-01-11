using System;
using GyverMatrix.Helpers;
using Xamarin.Forms.Xaml;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectPage {
        public ConnectPage() =>
            InitializeComponent();


        private void ConnectButton_OnClicked(object sender, EventArgs e)
        {
            //using (var udpHelper = new UdpHelper())
            //{

                //udpHelper.Connect(IpAdress.Text,int.Parse(Port.Text));
            //}
          }


        private void AutoConnectSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {


                if (AutoConnectSwitch.IsToggled)
                {
                    using (var udpHelper = new UdpHelper())
                    {
                        udpHelper.Connect(IpAdress.Text, int.Parse(Port.Text));
                        udpHelper.Send("$4 0 255;");
                    }
                } else {
                    using (var udpHelper = new UdpHelper())
                    {
                        udpHelper.Connect(IpAdress.Text, int.Parse(Port.Text));
                        udpHelper.Send("$4 0 20;");
                    }
                }

            
        }
    }
}