namespace GyverMatrix.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class GamesPage
{
    public GamesPage() =>
        InitializeComponent();

    private async Task SetBrightnesstAsync()
    {
        await UdpHelper.Send("$4 0 " + (int)BrightnessSlider.Value + ";");
        await SecureStorage.SetAsync("BR", ((int)BrightnessSlider.Value).ToString());
    }

    private async Task SetSpeedAsync()
    {
        await UdpHelper.Send("$15 " + (int)SpeedSlider.Value + " 2;");
    }

    private async void GameSwitch_PropertyChanged(
        object sender,
        PropertyChangedEventArgs e)
    {
        int num = Games.SelectedIndex;

        if (num <= -1)
            return;
        var mode = GameSwitch.IsToggled ? "1" : "0";

        if (mode == await SecureStorage.GetAsync("GSW" + num))
            return;
        switch (mode)
        {
            case "0":
                await UdpHelper.Send("$9 1 " + num + " 0;");
                await SecureStorage.SetAsync("GSW" + num, mode);
                break;
            case "1":
                await UdpHelper.Send("$9 1 " + num + " 1;");
                await SecureStorage.SetAsync("GSW" + num, mode);
                break;
        }
    }

    private async void DemoSwitch_PropertyChanged(
        object sender,
        PropertyChangedEventArgs e)
    {
        int num = Games.SelectedIndex;

        if (num <= -1)
            return;
        var mode = DemoSwitch.IsToggled ? "1" : "0";

        if (mode == await SecureStorage.GetAsync("DSW" + num))
            return;
        switch (mode)
        {
            case "0":
                await UdpHelper.Send("$9 2 " + num + " 0;");
                await SecureStorage.SetAsync("DSW" + num, mode);
                break;
            case "1":
                await UdpHelper.Send("$9 2 " + num + " 1;");
                await SecureStorage.SetAsync("DSW" + num, mode);
                break;
        }
    }

    private async void ContentPage_Appearing(
        object sender,
        EventArgs e)
    {
        string gamesListText = await SecureStorage.GetAsync("Games");
        string[] gamesList = gamesListText.Split(',');
        if (Games.Items.Count != gamesList.Length)
        {
            Games.Items.Clear();
            foreach (var t in gamesList)
            {
                Games.Items.Add(t);
            }
        }

        if (Games.SelectedIndex >= 0)
            return;
        GS.IsVisible = false;
        DS.IsVisible = false;
        SS.IsVisible = false;
        BS.IsVisible = false;
        Grid.IsVisible = false;
    }

    private async void Games_SelectedIndexChanged(
        object sender,
        EventArgs e)
    {

        GS.IsVisible = true;
        DS.IsVisible = true;
        SS.IsVisible = true;
        BS.IsVisible = true;
        Grid.IsVisible = true;
        //Дим, это штука адаптирует размеры кнопок под размеры экрана
        double x = stackLayot.Width;

        CD1.Width = x / 3;
        CD2.Width = x / 3;
        RD1.Height = x / 3;
        RD2.Height = x / 3;

        Console.WriteLine(x);

        //выбор игры

        int num = Games.SelectedIndex;

        int a;
        //string ack = "";
        string text = "";

        do
        {
            await UdpHelper.Send("$9 0 " + num + ";");
            string ack = await ParseHelper.Effects(await UdpHelper.Receive());

            if (ack == "ack")
            {
                a = 0;
            }
            else { a = 1; text = ack; Console.WriteLine(ack); }
        }
        while (a == 0);
        Console.WriteLine(text);
        string sg1 = text.Split('|')[3].Split(':')[1];
        string br1 = text.Split('|')[2].Split(':')[1];
        string ug1 = text.Split('|')[6].Split(':')[1];
        string gs1 = text.Split('|')[1].Split(':')[1];
        Console.WriteLine(sg1);
        Console.WriteLine(br1);
        Console.WriteLine(ug1);
        Console.WriteLine(gs1);


        SpeedSlider.Value = int.Parse(sg1);

        BrightnessSlider.Value = int.Parse(br1);

        DemoSwitch.IsToggled = ug1 == "1";
        await SecureStorage.SetAsync("DSW" + num, ug1);
        GameSwitch.IsToggled = sg1 == "1";
        await SecureStorage.SetAsync("GSW" + num, gs1);
    }

    private async void TapGestureRecognizer0_Tapped(
        object sender,
        EventArgs e)
    {
        await UdpHelper.Send("$10;");
    }

    private async void TapGestureRecognizer1_Tapped(
        object sender,
        EventArgs e)
    {
        await UdpHelper.Send("$11;");

    }

    private async void TapGestureRecognizer2_Tapped(
        object sender,
        EventArgs e)
    {
        await UdpHelper.Send("$13;");
    }

    private async void TapGestureRecognizer3_Tapped(
        object sender,
        EventArgs e)
    {
        await UdpHelper.Send("$12;");
    }

    private async void BrightnessSlider_ValueChanged(
        object sender,
        ValueChangedEventArgs e)
    {
        BrightnessText.Text = ((int)((Slider)sender).Value).ToString();
        await SetBrightnesstAsync();
    }

    private async void SpeedSlider_ValueChanged(
        object sender,
        ValueChangedEventArgs e)
    {
        SpeedText.Text = ((int)((Slider)sender).Value).ToString();
        await SetSpeedAsync();
    }
}