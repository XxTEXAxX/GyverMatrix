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

            //запрос настроек

            UdpHelper.Send("$18 1;");
            ParseHelper.SetSettingsAsync(UdpHelper.Receive());

            //запрос эффектов

            UdpHelper.Send("$18 98;");
            ParseHelper.SetEffects(UdpHelper.Receive());

            //запрос игр

            UdpHelper.Send("$18 99;");
            ParseHelper.SetGames(UdpHelper.Receive());

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