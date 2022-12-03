using GyverMatrix.Helpers;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GyverMatrix.Pages {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EffectsPage {
        public EffectsPage() {
            InitializeComponent();
            HS.IsVisible = false;
            ES.IsVisible = false;
            SS.IsVisible = false;
            BS.IsVisible = false;
        }

        private async Task SetBrightnesstAsync() {
            await UdpHelper.Send("$4 0 " + (int)BrightnessSlider.Value + ";");
            await SecureStorage.SetAsync("BR", ((int)BrightnessSlider.Value).ToString());
        }
        private async Task SetSpeedAsync() {
            await UdpHelper.Send("$15 " + (int)SpeedSlider.Value + " 0;");
        }
        private async void EffectSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            int num = Effects.SelectedIndex;

            if (num <= -1)
                return;
            var mode = EffectSwitch.IsToggled ? "1" : "0";

            if (mode == await SecureStorage.GetAsync("ESW" + num))
                return;
            switch (mode) {
                case "0":
                    await UdpHelper.Send("$8 2 " + num + " 0;");
                    await SecureStorage.SetAsync("ESW" + num, mode);
                    Console.WriteLine("SwitchChanged" + "$8 2 " + num + " 0;");
                    break;
                case "1":
                    await UdpHelper.Send("$8 2 " + num + " 1;");
                    await SecureStorage.SetAsync("ESW" + num, mode);
                    Console.WriteLine("SwitchChanged" + "$8 2 " + num + " 1;");
                    break;
            }
        }
        private async void HourSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            int num = Effects.SelectedIndex;

            var mode = HourSwitch.IsToggled ? "1" : "0";

            if (mode == await SecureStorage.GetAsync("HSW" + num))
                return;
            switch (mode) {
                case "0":
                //await UdpHelper.Send("$16 0;"); //добавлю позже
                case "1":
                    //await UdpHelper.Send("$19 0 1;"); //добавлю позже
                    await SecureStorage.SetAsync("HSW" + num, mode);
                    break;
            }
        }
        private async void BrightnessSlider_ValueChanged(object sender, ValueChangedEventArgs e) {
            BrightnessText.Text = ((int)((Slider)sender).Value).ToString();
            await SetBrightnesstAsync();
        }
        private async void SpeedSlider_ValueChanged(object sender, ValueChangedEventArgs e) {
            SpeedText.Text = ((int)((Slider)sender).Value).ToString();
            await SetSpeedAsync();
        }

        private async void ContentPage_Appearing(object sender, EventArgs e) {

            string effectsListText = await SecureStorage.GetAsync("Effects");
            string[] effectsList = effectsListText.Split(',');
            if (Effects.Items.Count != effectsList.Length) {
                Effects.Items.Clear();
                foreach (var t in effectsList) {
                    Effects.Items.Add(t);
                }
            }

            if (Effects.SelectedIndex >= 0)
                return;
            ES.IsVisible = false;
            HS.IsVisible = false;
            SS.IsVisible = false;
            BS.IsVisible = false;
        }



        private async void Effects_SelectedIndexChanged(object sender, EventArgs e) {
            int num = Effects.SelectedIndex;

            if (num > -1) {

                ES.IsVisible = true;
                HS.IsVisible = true;
                SS.IsVisible = true;
                BS.IsVisible = true;

                //await UdpHelper.Send("$8 0 " + num + ";");
                int a;
                string message = "";
                do {
                    await UdpHelper.Send("$8 0 " + num + ";");
                    string ack = await ParseHelper.Effects(await UdpHelper.Receive());

                    if (ack == "ack") {
                        a = 0;
                    } else { a = 1; message = ack; Console.WriteLine(ack); }
                }
                while (a == 0);
                //string debug = await ParseHelper.Effects(await UdpHelper.Receive()); // ДИМА, НЕ УДАЛЯЙ!!!! без него он возвращает ack

                //string message = await ParseHelper.Effects(num);
                Console.WriteLine(message);
                if (message != "") {
                    Console.WriteLine("я добрался до сюда");
                    string[] message1 = message.Split('|');

                    foreach (var t in message1)
                    {
                        Console.WriteLine(t);
                    }

                    BrightnessSlider.Value = int.Parse(message1[2].Split(':')[1]);
                    if (message1[3].Split(':')[1] != "X") {
                        SpeedSlider.Value = int.Parse(message1[3].Split(':')[1]);
                    }

                    int x = 0;
                    if (message1[6].Split(':')[1] != "X") {
                        x = int.Parse(message1[6].Split(':')[1]);

                    } else {
                        HS.IsVisible = false;
                        SS.IsVisible = false;
                    }
                    int y = int.Parse(message1[7].Split(':')[1]);

                    await SecureStorage.SetAsync("HSW" + num, message1[6].Split(':')[1]);
                    await SecureStorage.SetAsync("ESW" + num, message1[7].Split(':')[1]);

                    HourSwitch.IsToggled = x == 1;

                    EffectSwitch.IsToggled = y == 1;


                }
            } else {
                HS.IsVisible = false;
                ES.IsVisible = false;
                SS.IsVisible = false;
                BS.IsVisible = false;
            }


            Console.WriteLine(Effects.SelectedIndex);
        }
    }
}