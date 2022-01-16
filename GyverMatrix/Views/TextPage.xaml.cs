using GyverMatrix.Helpers;
using System;
using System.Threading.Tasks;
using System.Drawing;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Color = Xamarin.Forms.Color;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TextPage {

        bool load = false;
        public TextPage() =>
            InitializeComponent();

        private int red = 0;
        private int green = 0;
        private int blue = 0;
        private async Task SetBrightnesstAsync()
        {
            await UdpHelper.Send("$4 0 " + (int)BrightnessSlider.Value + ";");
            await SecureStorage.SetAsync("BR", ((int)BrightnessSlider.Value).ToString());
        }

        private async Task ColorChanged()
        {

            Box.Fill = Color.FromRgb(red,green,blue);
            

            string col = Color.FromRgb(red, green, blue).ToHex().Remove(0,3);
            await UdpHelper.Send("$0 " + col + ";");
            Console.WriteLine(col);

        }
        private async Task SetSpeedAsync()
        {
            await UdpHelper.Send("$15 " + (int)SpeedSlider.Value + " 1;");
        }
        private async void Stop_Clicked(object sender, EventArgs e) {
            await UdpHelper.Send("$7 0;");
        }
        private async void Start_Clicked(object sender, EventArgs e) {
            await UdpHelper.Send("$7 1;");
        }
        private async void Send_Clicked(object sender, EventArgs e) {
            if (Text.Text != "") {
                await UdpHelper.Send("$6 0|" + Text.Text );
            }
        }
        private async void SpeedSlider_ValueChanged(object sender, ValueChangedEventArgs e) {
            //Console.W
            SpeedText.Text = ((int)((Slider)sender).Value).ToString();
            if (load)
            {
                await SetSpeedAsync();
            }
        }
        private async void BrightnessSlider_ValueChanged(object sender, ValueChangedEventArgs e) {
            BrightnessText.Text = ((int)((Slider)sender).Value).ToString();
            await SetBrightnesstAsync();
        }

        private async void BlueSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            BlueText.Text = ((int)((Slider)sender).Value).ToString();
            blue = (int)((Slider)sender).Value;
            await ColorChanged();
        }

        private async void GreenSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            GreenText.Text = ((int)((Slider)sender).Value).ToString();
            green = (int)((Slider)sender).Value;
            await ColorChanged();
        }

        private async void RedSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            RedText.Text = ((int)((Slider)sender).Value).ToString();
            red = (int)((Slider)sender).Value;
            await ColorChanged();
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {

            load = false;

            await UdpHelper.Send("$7 1;");
            await UdpHelper.Send("$18 4;");
            string text = await ParseHelper.Text(await UdpHelper.Receive());
            string[] settings = text.Split('|');

            string demo = settings[4].Split(':')[1];
            string type = settings[7].Split(':')[1];

            await SecureStorage.SetAsync("ST", settings[1].Split(':')[1]);
            await SecureStorage.SetAsync("DemoT", settings[4].Split(':')[1]);
            await SecureStorage.SetAsync("TypeT", settings[7].Split(':')[1]);

            BrightnessSlider.Value = int.Parse(settings[0].Split(':')[1]);
            SpeedSlider.Value = int.Parse(settings[1].Split(':')[1]);

            if (demo == "1")
            {
                DemoSwitch.IsToggled = true;
            }
            else
            {
                DemoSwitch.IsToggled = false;
            }

            Col.SelectedIndex = int.Parse(type);


            for(int i = 0; i < settings.Length; i++)
            {
                Console.WriteLine(settings[i]);
            }
            if (settings[3].Split(':')[1] != "[]")
            {
                string a = settings[3].Split(':')[1].Remove(0, 1);
                a = a.Remove(a.Length - 2, a.Length - (a.Length - 2));

                Text.Text = a;
            }
            //Console.WriteLine(a);
            load = true;
        }

        private async void DemoSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            string Mode;

            if (DemoSwitch.IsToggled)
            {
                Mode = "1";
            }
            else
            {
                Mode = "0";
            }

            if (Mode != await SecureStorage.GetAsync("DemoT"))
            {
                if (Mode == "0")
                {
                    await UdpHelper.Send("$7 3;"); 
                    await SecureStorage.SetAsync("DemoT", Mode);
                }
                else if (Mode == "1")
                {
                    await UdpHelper.Send("$7 2;"); 
                    await SecureStorage.SetAsync("DemoT", Mode);
                }
            }
        }

        private async void Col_SelectedIndexChanged(object sender, EventArgs e)
        {

            Console.WriteLine("ошибка тут");
            string type = await SecureStorage.GetAsync("TypeT");

            int num = Col.SelectedIndex;

            if (int.Parse(type) != num)
            {
                await UdpHelper.Send("$7 4 "+num+";");
                await SecureStorage.SetAsync("TypeT", num.ToString());
            }

            if(num == 0)
            {
                ColorPicker.IsVisible = true;
            }
            else
            {
                ColorPicker.IsVisible = false;
            }

        }
    }
}