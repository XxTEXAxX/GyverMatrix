using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GyverMatrix.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectPage : INotifyPropertyChanged {
        public ConnectPage() =>
            InitializeComponent();
        private bool _a;
        public FlyoutBehavior FlyoutBehavior { get; private set; } = FlyoutBehavior.Disabled;

        private async void ConnectButton_OnClicked(object sender, EventArgs e) {
            UdpHelper.Connect(IpAdress.Text, int.Parse(Port.Text));

            //запрос настроек
            await UdpHelper.Send("$18 1;");
            await ParseHelper.SetSettings(await UdpHelper.Receive());

            //запрос эффектов
            await UdpHelper.Send("$18 99;");
            await ParseHelper.SetEffects(await UdpHelper.Receive());

            //запрос игр
            await UdpHelper.Send("$18 98;");
            await ParseHelper.SetGames(await UdpHelper.Receive());

            //запрос настроек сети 
            await UdpHelper.Send("$18 9;");
            await ParseHelper.SetSettingsNet(await UdpHelper.Receive());

            _a = true;
            FlyoutBehavior = FlyoutBehavior.Flyout;
            NotifyPropertyChanged(nameof(FlyoutBehavior));
        }
        private async void AutoConnectSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (!_a)
                return;
            //await UdpHelper.Send(AutoConnectSwitch.IsToggled ? "$4 0 255;" : "$4 0 20;");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}