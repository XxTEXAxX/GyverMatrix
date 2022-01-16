using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GyverMatrix.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectPage : INotifyPropertyChanged {
        public ConnectPage() =>
            InitializeComponent();
        private bool _a;
        public FlyoutBehavior FlyoutBehavior { get; private set; } = FlyoutBehavior.Disabled;

        bool load = false;
        private async void Connect()
        {
            if (ConnectHelper.connected == false)
            {
                ButCon.BackgroundColor = Color.DarkOrange;
                ButCon.Text = "Подключение...";
                if (UdpHelper.Connect(IpAdress.Text, int.Parse(Port.Text)))
                {

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
                }
                else
                {
                    ButCon.BackgroundColor = Color.Red;
                    ButCon.Text = "Не подключено";
                }
            }
            else if (ConnectHelper.connected == true)
            {
                ButCon.BackgroundColor = Color.Blue;
                ButCon.Text = "Подключить";
                UdpHelper.CloseConnect();

            }
            _a = true;
            FlyoutBehavior = FlyoutBehavior.Flyout;
            NotifyPropertyChanged(nameof(FlyoutBehavior));
        }

        private async void ConnectButton_OnClicked(object sender, EventArgs e) {
            Connect();
        }
        private async void AutoConnectSwitch_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (load)
            {
                if (AutoConnectSwitch.IsToggled)
                {
                    await SecureStorage.SetAsync("AutoConnect", "1");
                }
                else
                {
                    await SecureStorage.SetAsync("AutoConnect", "0");
                }
            }

            //if (!_a)
            //    return;
            //await UdpHelper.Send(AutoConnectSwitch.IsToggled ? "$4 0 255;" : "$4 0 20;");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private async void Page_Appearing(object sender, EventArgs e)
        {
            load = false;

            string autoconnect = await SecureStorage.GetAsync("AutoConnect");

            if(autoconnect == "1")
            {
                AutoConnectSwitch.IsToggled = true;

                if (!ConnectHelper.connected)
                {
                    Connect();
                }
            }
            else
            {
                AutoConnectSwitch.IsToggled = false;
            }

            load = true;
        }
    }
}