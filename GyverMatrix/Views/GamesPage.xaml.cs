using GyverMatrix.Helpers;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamesPage {
        
        public GamesPage() {
            
            InitializeComponent();
        }

        private async Task SetBrightnesstAsync()
        {
            await UdpHelper.Send("$4 0 " + (int)BrightnessSlider.Value + ";");
            await SecureStorage.SetAsync("BR", ((int)BrightnessSlider.Value).ToString());
        }

        private async Task SetSpeedAsync()
        {
            await UdpHelper.Send("$15 " + (int)SpeedSlider.Value + " 2;");
        }
        private async void GameSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            string Mode;
            int num = Games.SelectedIndex;

            if (num > -1)
            {

                if (GameSwitch.IsToggled)
                {
                    Mode = "1";
                }
                else
                {
                    Mode = "0";
                }

                if (Mode != await SecureStorage.GetAsync("GSW" + num))
                {
                    if (Mode == "0")
                    {
                        await UdpHelper.Send("$9 1 " + num + " 0;");
                        await SecureStorage.SetAsync("GSW" + num, Mode);
                        
                    }
                    else if (Mode == "1")
                    {
                        await UdpHelper.Send("$9 1 " + num + " 1;");
                        await SecureStorage.SetAsync("GSW" + num, Mode);
                        
                    }
                }
            }
        }

        private async void DemoSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            string Mode;
            int num = Games.SelectedIndex;

            if (num > -1)
            {

                if (DemoSwitch.IsToggled)
                {
                    Mode = "1";
                }
                else
                {
                    Mode = "0";
                }

                if (Mode != await SecureStorage.GetAsync("DSW" + num))
                {
                    if (Mode == "0")
                    {
                        await UdpHelper.Send("$9 2 " + num + " 0;");
                        await SecureStorage.SetAsync("DSW" + num, Mode);
                       
                    }
                    else if (Mode == "1")
                    {
                        await UdpHelper.Send("$9 2 " + num + " 1;");
                        await SecureStorage.SetAsync("DSW" + num, Mode);
                        
                    }
                }
            }
        }

        private async void ContentPage_Appearing(object sender, System.EventArgs e)
        {
            string GamesListText = await SecureStorage.GetAsync("Games");
            string[] GamesList = GamesListText.Split(',');
            if (Games.Items.Count != GamesList.Length)
            {
                Games.Items.Clear();
                for (int i = 0; i < GamesList.Length; i++)
                {
                    
                    Games.Items.Add(GamesList[i]);
                }
            }
            if (Games.SelectedIndex < 0) {
                GS.IsVisible = false;
                DS.IsVisible = false;
                SS.IsVisible = false;
                BS.IsVisible = false;
            }
        }

        private async void Games_SelectedIndexChanged(object sender, EventArgs e)
        {

            GS.IsVisible = true;
            DS.IsVisible = true;
            SS.IsVisible = true; 
            BS.IsVisible = true;
            //Дим, это штука адаптирует размеры кнопок под размеры экрана
            double x = stackLayot.Width;

            CD1.Width = x / 3;
            CD2.Width = x / 3;
            RD1.Height = x / 3;
            RD2.Height = x / 3;
            
            Console.WriteLine(x);

            //выбор игры

            int num = Games.SelectedIndex;

            await UdpHelper.Send("$9 0 " + num + ";");
            string text = await UdpHelper.Receive();
            Console.WriteLine(text);
            string SG1 = text.Split(' ',';')[1].Split('|')[3].Split(':')[1];
            string BR1 = text.Split(' ', ';')[1].Split('|')[2].Split(':')[1];
            string UG1 = text.Split(' ', ';')[1].Split('|')[6].Split(':')[1];
            string GS1 = text.Split(' ', ';')[1].Split('|')[1].Split(':')[1];
            Console.WriteLine(SG1);
            Console.WriteLine(BR1);
            Console.WriteLine(UG1);
            Console.WriteLine(GS1);


            SpeedSlider.Value = int.Parse(SG1);

            BrightnessSlider.Value = int.Parse(BR1);

            if (UG1 == "1") {
                DemoSwitch.IsToggled = true;
            }
            else
            {
                DemoSwitch.IsToggled = false;
            }
            await SecureStorage.SetAsync("DSW" + num, UG1);
            if (SG1 == "1")
            {
                GameSwitch.IsToggled = true;
            }
            else
            {
                GameSwitch.IsToggled = false;
            }
            await SecureStorage.SetAsync("GSW" + num, GS1);
        }

        private async void TapGestureRecognizer0_Tapped(object sender, EventArgs e)
        {
            await UdpHelper.Send("$10;");
        }
        private async void TapGestureRecognizer1_Tapped(object sender, EventArgs e)
        {
            await UdpHelper.Send("$11;");

        }
        private async void TapGestureRecognizer2_Tapped(object sender, EventArgs e)
        {
            await UdpHelper.Send("$13;");
        }
        private async void TapGestureRecognizer3_Tapped(object sender, EventArgs e)
        {
            await UdpHelper.Send("$12;");
        }

        private async void BrightnessSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            BrightnessText.Text = ((int)((Slider)sender).Value).ToString();
            await SetBrightnesstAsync();
        }

        private async void SpeedSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SpeedText.Text = ((int)((Slider)sender).Value).ToString();
            await SetSpeedAsync();
        }
    }
}