using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Threading.Tasks;
using GyverMatrix.Helpers;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage {

        //проблемы с рандомом, исправлю завтра
        public SettingsPage() =>
            InitializeComponent();
        private async Task SetBrightnesstAsync() {
            await UdpHelper.Send("$4 0 " + (int)BrightnessSlider.Value + ";");
            await SecureStorage.SetAsync("BR", ((int)BrightnessSlider.Value).ToString());
        }

        private async Task SetAuto()
        {
            string PD = await SecureStorage.GetAsync("PD");
            string IT = await SecureStorage.GetAsync("IT");
            Console.WriteLine("установка таймеров");
            if (PD != AutoTime1.Text | IT != AutoTime2.Text)
            {
                await UdpHelper.Send("$17 " + int.Parse(AutoTime1.Text) + " " + int.Parse(AutoTime2.Text) + ";");
                Console.WriteLine("$17 " + int.Parse(AutoTime1.Text) + " " + int.Parse(AutoTime2.Text) + ";");
            }
            else { Console.WriteLine("не выполнено"); }
        }

        private async void DemoSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {

            string Mode;

            if (DemoSwitch.IsToggled)
            {
                Mode = "1";
            }
            else
            {
                Mode = "0";
            }

            if (Mode != await SecureStorage.GetAsync("DM"))
            {
                if (Mode == "0")
                {
                    await UdpHelper.Send("$16 1;");
                    await SecureStorage.SetAsync("DM", Mode);
                }
                else if (Mode == "1")
                {
                    await UdpHelper.Send("$16 0;");
                    await SecureStorage.SetAsync("DM", Mode);
                }
            }

        }
        private async void Undo_Clicked(object sender, EventArgs e) {
            await UdpHelper.Send("$16 2;");
        }
        private async void Next_Clicked(object sender, EventArgs e) {
            await UdpHelper.Send("$16 3;");
        }
        private async void AutoCheck1_Clicked(object sender, EventArgs e) {
            await SetAuto();
        }
        private async void AutoCheck2_Clicked(object sender, EventArgs e) {
            await SetAuto();
        }
        private async void AutoCheck3_Clicked(object sender, EventArgs e) {
            string PW = await SecureStorage.GetAsync("PW");

            if (PW != Curret.Text)
            {
                await UdpHelper.Send("$23 0 " + Curret.Text + ";");
                await SecureStorage.SetAsync("PW", Curret.Text);
            }
        }
        private async void NetSet1_Clicked(object sender, EventArgs e) {

            string name1 = await SecureStorage.GetAsync("AN");
            string pass1 = await SecureStorage.GetAsync("AA");

            string name2 = NameEntry1.Text;
            string pass2 = PassEntry1.Text;

            if(name1 != name2 | pass1 != pass2)
            {
                await UdpHelper.Send("$6 4|" + name2);
                await UdpHelper.Send("$6 5|" + pass2);
            }


        }
        private void NetSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {

        }
        private void NetSet2_Clicked(object sender, EventArgs e) {

        }
        private async void BrightnessSlider_OnValueChanged(object sender, ValueChangedEventArgs e) {
            BrightnessText.Text = ((int)((Slider)sender).Value).ToString();
            await SetBrightnesstAsync();
        }
        private async void AutoSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            string Mode;

            if (AutoSwitch.IsToggled)
            {
                Mode = "1";
            }
            else
            {
                Mode = "0";
            }

            if (Mode != await SecureStorage.GetAsync("AP"))
            {
                await UdpHelper.Send("$16 4 " + Mode + ";");
                await SecureStorage.SetAsync("AP", Mode);
            }


        }
        private async void RandomSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            string Mode;

            if (RandomSwitch.IsToggled)
            {
                Mode = "1";
            }
            else
            {
                Mode = "0";
            }

            if (Mode != await SecureStorage.GetAsync("RM"))
            {
                await UdpHelper.Send("$16 5 " + Mode + ";");
                await SecureStorage.SetAsync("RM", Mode);
            }
        }
        private async void SettingsPage_OnAppearing(object sender, EventArgs e) {
            //выставление настроек яркости из хранилища

            string w = await SecureStorage.GetAsync("W");
            string h = await SecureStorage.GetAsync("H");
            SettingsPageTitle.Title = "Матрица " + w + "x" + h;
            int result;
            int.TryParse(await SecureStorage.GetAsync("BR"), out result);
            BrightnessSlider.Value = result;

            //выставление прочих настроек из хранилища
            Curret.Text = await SecureStorage.GetAsync("PW");

            //Выставление настроек режимов из хранилища 


            string DM = await SecureStorage.GetAsync("DM"); // DM:Х        демо режим, где Х = 0 - выкл (ручное управление); 1 - вкл
            string AP = await SecureStorage.GetAsync("AP"); // AP:Х        автосменарежимов, где Х = 0 - выкл; 1 - вкл
            string RM = await SecureStorage.GetAsync("RM"); // RM:Х        смена режимов в случайном порядке, где Х = 0 - выкл; 1 - вкл
            string PD = await SecureStorage.GetAsync("PD"); // PD:число    продолжительность режима в секундах
            string IT = await SecureStorage.GetAsync("IT"); // IT:число    время бездействия в секундах

            if (DM == "1") { DemoSwitch.IsToggled = true; } else { DemoSwitch.IsToggled = false; }
            if (AP == "1") { AutoSwitch.IsToggled = true; } else { AutoSwitch.IsToggled = false; }
            if (RM == "1") { RandomSwitch.IsToggled = true; } else { RandomSwitch.IsToggled = false; }
            AutoTime1.Text = PD;
            AutoTime2.Text = IT;

            //Выставление настроек сети из хранилища 

            NameEntry1.Text = await SecureStorage.GetAsync("AN");
            PassEntry1.Text = await SecureStorage.GetAsync("AA");

            string AU = await SecureStorage.GetAsync("AU"); 
            if (AU == "1") { NetSwitch.IsToggled = true; } else { NetSwitch.IsToggled = false; }

            NameEntry2.Text = await SecureStorage.GetAsync("NW");
            PassEntry2.Text = await SecureStorage.GetAsync("NA");
            IpEntry2.Text = await SecureStorage.GetAsync("IP");
        }
    }
}