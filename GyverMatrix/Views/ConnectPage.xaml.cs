using System;
using GyverMatrix.Helpers;
using Xamarin.Forms.Xaml;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectPage {
        public ConnectPage() =>
            InitializeComponent();

        private async void ConnectButton_OnClicked(object sender, EventArgs e)
        {
            using (var udpHelper = new UdpHelper("852837632",45))
            {
                await udpHelper.Send("");
            }
        }
    }
}