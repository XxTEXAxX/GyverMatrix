namespace GyverMatrix.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ConnectPage : INotifyPropertyChanged
{
    #region Initialization

    public ConnectPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void Page_Appearing(object sender, EventArgs e)
    {
        //string autoconnect = await SecureStorage.GetAsync("AutoConnect");

        IpAdress.Text = await SecureStorage.GetAsync("IpAdress");
        Port.Text = await SecureStorage.GetAsync("Port");
        Application.Current.UserAppTheme = await SecureStorage.GetAsync("Theme") switch
        {
            "dark" => OSAppTheme.Dark,
            "light" => OSAppTheme.Light,
            _ => OSAppTheme.Unspecified
        };
        ThemeSegmentedControl.SelectedSegment = (int)Application.Current.UserAppTheme;
        //if (autoconnect == "1")
        //{
        //    AutoConnectSwitch.IsToggled = true;
        //    if (!ConnectHelper.connected)
        //    {
        //        await Connect();
        //    }
        //}
        //else
        //{
        //    AutoConnectSwitch.IsToggled = false;
        //}
    }

    #endregion

    #region Properties

    public FlyoutBehavior FlyoutBehavior { get; private set; } = FlyoutBehavior.Disabled;

    public LayoutState CurrentLayoutState
    {
        get => _currentLayoutState;
        private set
        {
            _currentLayoutState = value;
            NotifyPropertyChanged();
        }
    }

    public bool NavBarIsVisible
    {
        get => _navBarIsVisible;
        private set
        {
            _navBarIsVisible = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Fields

    private bool _load, _navBarIsVisible;
    private LayoutState _currentLayoutState = LayoutState.None;

    #endregion

    #region Private Methods
    private async Task Connect()
    {
        //if (!ConnectHelper.connected)
        //{
        //    if (UdpHelper.Connect(IpAdress.Text, int.Parse(Port.Text)))
        //    {

        //        //запрос настроек
        //        await UdpHelper.Send("$18 1;");
        //        await ParseHelper.SetSettings(await UdpHelper.Receive());

        //        //запрос эффектов
        //        await UdpHelper.Send("$18 99;");
        //        await ParseHelper.SetEffects(await UdpHelper.Receive());

        //        //запрос игр
        //        await UdpHelper.Send("$18 98;");
        //        await ParseHelper.SetGames(await UdpHelper.Receive());

        //        //запрос настроек сети 
        //        await UdpHelper.Send("$18 9;");
        //        await ParseHelper.SetSettingsNet(await UdpHelper.Receive());

        //        ButCon.BackgroundColor = Color.Green;
        //        NavBarIsVisible = true;
        //        ButCon.Text = "Подключено";
        //    }
        //    else
        //    {
        //        ButCon.BackgroundColor = Color.Red;
        //        ButCon.Text = "Не подключено";
        //    }
        //}
        //else
        //{
        //    ButCon.BackgroundColor = Color.Blue;
        //    ButCon.Text = "Подключить";
        //    UdpHelper.CloseConnect();
        //}
        FlyoutBehavior = FlyoutBehavior.Flyout;
        NotifyPropertyChanged(nameof(FlyoutBehavior));
    }
    #endregion

    #region Events

    private async void ConnectButton_OnClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(IpAdress.Text) || string.IsNullOrEmpty(Port.Text))
        {
            await DisplayAlert("Ошибка", "Заполните поля", "Закрыть");
            return;
        }
        CurrentLayoutState = LayoutState.Loading;
        await Connect();
        await SecureStorage.SetAsync("IpAdress", IpAdress.Text);
        await SecureStorage.SetAsync("Port", Port.Text);
        CurrentLayoutState = LayoutState.None;
    }

    private async void AutoConnectSwitch_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (!_load)
            return;
        if (AutoConnectSwitch.IsToggled)
        {
            await SecureStorage.SetAsync("AutoConnect", "1");
        }
        else
        {
            await SecureStorage.SetAsync("AutoConnect", "0");
        }
    }

    #endregion

    #region IPC Realization

    public new event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    #endregion

    private async void Theme_ValueChanged(object sender, SegmentedControl.FormsPlugin.Abstractions.ValueChangedEventArgs e)
    {
        if (!_load)
        {
            _load = true;
            return;
        }
        switch (e.NewValue)
        {
            case 1:
                Application.Current.UserAppTheme = OSAppTheme.Light;
                await SecureStorage.SetAsync("Theme", "light");
                break;
            case 2:
                Application.Current.UserAppTheme = OSAppTheme.Dark;
                await SecureStorage.SetAsync("Theme", "dark");
                break;
            default:
                Application.Current.UserAppTheme = OSAppTheme.Dark;
                await SecureStorage.SetAsync("Theme", string.Empty);
                break;
        }
    }

    void Language_ValueChanged(object sender, SegmentedControl.FormsPlugin.Abstractions.ValueChangedEventArgs e)
    {
    }
}