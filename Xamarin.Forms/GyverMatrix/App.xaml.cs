namespace GyverMatrix;

public partial class App
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }

    private async void Application_PageAppearing(object sender, Page e)
    {
        UserAppTheme = await SecureStorage.GetAsync("Theme") switch
        {
            "dark" => OSAppTheme.Dark,
            "light" => OSAppTheme.Light,
            _ => OSAppTheme.Unspecified
        };
    }
}