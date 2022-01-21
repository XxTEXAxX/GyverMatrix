using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GyverMatrix.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectPage : INotifyPropertyChanged {
        public ConnectPage() =>
            InitializeComponent();
        public FlyoutBehavior FlyoutBehavior { get; private set; } = FlyoutBehavior.Disabled;

        bool _load;
        private async Task Connect() {
            if (!ConnectHelper.connected && Port.Text != "" && IpAdress.Text != "") {
                ButCon.BackgroundColor = Color.DarkOrange;
                ButCon.Text = "Подключение...";
                if (UdpHelper.Connect(IpAdress.Text, int.Parse(Port.Text))) {

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

                    ButCon.BackgroundColor = Color.Green;
                    ButCon.Text = "Подключено";
                } else {
                    ButCon.BackgroundColor = Color.Red;
                    ButCon.Text = "Не подключено";
                }
            } else {
                ButCon.BackgroundColor = Color.Blue;
                ButCon.Text = "Подключить";
                UdpHelper.CloseConnect();
            }
            FlyoutBehavior = FlyoutBehavior.Flyout;
            NotifyPropertyChanged(nameof(FlyoutBehavior));
        }

        private async void ConnectButton_OnClicked(object sender, EventArgs e) {
            await Connect();
            await SecureStorage.SetAsync("IpAdress", IpAdress.Text);
            await SecureStorage.SetAsync("Port", Port.Text);
        }
        private async void AutoConnectSwitch_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (!_load)
                return;
            if (AutoConnectSwitch.IsToggled) {
                await SecureStorage.SetAsync("AutoConnect", "1");
            } else {
                await SecureStorage.SetAsync("AutoConnect", "0");
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private async void Page_Appearing(object sender, EventArgs e) {
            _load = false;

            string autoconnect = await SecureStorage.GetAsync("AutoConnect");

            IpAdress.Text = await SecureStorage.GetAsync("IpAdress");
            Port.Text = await SecureStorage.GetAsync("Port");

            if (autoconnect == "1") {
                AutoConnectSwitch.IsToggled = true;
                if (!ConnectHelper.connected) {
                    await Connect();
                }
            } else {
                AutoConnectSwitch.IsToggled = false;
            }
            _load = true;
        }
    }
}