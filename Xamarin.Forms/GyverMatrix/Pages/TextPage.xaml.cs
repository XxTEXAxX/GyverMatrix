namespace GyverMatrix.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class TextPage
{
    bool _load;

    public TextPage() =>
        InitializeComponent();

    private async Task SetBrightnesstAsync()
    {
        await UdpHelper.Send("$4 0 " + (int)BrightnessSlider.Value + ";");
        await SecureStorage.SetAsync("BR", ((int)BrightnessSlider.Value).ToString());
    }

    private async Task SetSpeedAsync()
    {
        await UdpHelper.Send("$15 " + (int)SpeedSlider.Value + " 1;");
    }

    private async void Stop_Clicked(
        object sender,
        EventArgs e)
    {
        await UdpHelper.Send("$7 0;");
    }

    private async void Start_Clicked(
        object sender,
        EventArgs e)
    {
        await UdpHelper.Send("$7 1;");
    }

    private async void Send_Clicked(
        object sender,
        EventArgs e)
    {
        if (!string.IsNullOrEmpty(Text.Text))
        {
            await UdpHelper.Send("$6 0|" + Text.Text);
        }
    }

    private async void SpeedSlider_ValueChanged(
        object sender,
        ValueChangedEventArgs e)
    {
        //Console.W
        SpeedText.Text = ((int)((Slider)sender).Value).ToString();
        if (_load)
        {
            await SetSpeedAsync();
        }
    }

    private async void BrightnessSlider_ValueChanged(
        object sender,
        ValueChangedEventArgs e)
    {
        BrightnessText.Text = ((int)((Slider)sender).Value).ToString();
        await SetBrightnesstAsync();
    }

    private async void ContentPage_Appearing(
        object sender,
        EventArgs e)
    {
        _load = false;

        await UdpHelper.Send("$7 1;");
        await UdpHelper.Send("$18 4;");
        string text = ParseHelper.Text(await UdpHelper.Receive());
        string[] settings = text.Split('|');

        string demo = settings[4].Split(':')[1];
        string type = settings[7].Split(':')[1];

        await SecureStorage.SetAsync("ST", settings[1].Split(':')[1]);
        await SecureStorage.SetAsync("DemoT", settings[4].Split(':')[1]);
        await SecureStorage.SetAsync("TypeT", settings[7].Split(':')[1]);

        BrightnessSlider.Value = int.Parse(settings[0].Split(':')[1]);
        SpeedSlider.Value = int.Parse(settings[1].Split(':')[1]);

        DemoSwitch.IsToggled = demo == "1";

        Col.SelectedIndex = int.Parse(type);


        foreach (var t in settings)
        {
            Console.WriteLine(t);
        }
        if (settings[3].Split(':')[1] != "[]")
        {
            string a = settings[3].Split(':')[1].Remove(0, 1);
            a = a.Remove(a.Length - 1, a.Length - (a.Length - 1));

            Text.Text = a;
        }
        //Console.WriteLine(a);
        _load = true;
    }

    private async void DemoSwitch_PropertyChanged(
        object sender,
        PropertyChangedEventArgs e)
    {
        string mode = DemoSwitch.IsToggled ? "1" : "0";

        if (mode == await SecureStorage.GetAsync("DemoT"))
            return;
        switch (mode)
        {
            case "0":
                await UdpHelper.Send("$7 3;");
                await SecureStorage.SetAsync("DemoT", mode);
                break;
            case "1":
                await UdpHelper.Send("$7 2;");
                await SecureStorage.SetAsync("DemoT", mode);
                break;
        }
    }

    private async void Col_SelectedIndexChanged(
        object sender,
        EventArgs e)
    {

        Console.WriteLine("ошибка тут");
        string type = await SecureStorage.GetAsync("TypeT");

        int num = Col.SelectedIndex;

        if (int.Parse(type) != num)
        {
            await UdpHelper.Send("$7 4 " + num + ";");
            await SecureStorage.SetAsync("TypeT", num.ToString());
        }

        ColorPicker.IsVisible = num == 0;

    }

    private async void ColorTriangle_SelectedColorChanged(
        object sender,
        ColorPicker.BaseClasses.ColorPickerEventArgs.ColorChangedEventArgs e)
    {
        ColorTriangle colorTriangle = (ColorTriangle)sender;
        Box.Fill = colorTriangle.SelectedColor;
        string col = colorTriangle.SelectedColor.ToHex().Remove(0, 3);
        await UdpHelper.Send("$0 " + col + ";");
        Console.WriteLine(col);
    }
}