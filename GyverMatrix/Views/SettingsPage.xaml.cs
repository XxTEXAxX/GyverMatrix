using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Threading.Tasks;
using GyverMatrix.Helpers;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage {

        public SettingsPage()
        {
            InitializeComponent();

            StartAwaitAsync();


        }

        private async Task StartAwaitAsync()
        {
            //выставление настроек яркости из хранилища

            string W = await SecureStorage.GetAsync("W");
            string H = await SecureStorage.GetAsync("H");
            SettingsPageTitle.Title = "Матрица " + W + "x" + H;
            
            BrightnessSlider.Value = int.Parse(await SecureStorage.GetAsync("BR"));

            //выставление прочих настроек из хранилища

            string PW = await SecureStorage.GetAsync("PW");

            Curret.Text = PW;

            //Выставление настроек режимов из хранилища (доделаю завтра)

            
        }

        private async Task SetBrightnesstAsync()
        {

            UdpHelper.Send("$4 0 " + (int)BrightnessSlider.Value + ";");

            await SecureStorage.SetAsync("BR", ((int)BrightnessSlider.Value).ToString());
        }

        private void DemoSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {

        }

        private void Undo_Clicked(object sender, EventArgs e) {

        }

        private void Next_Clicked(object sender, EventArgs e) {

        }

        private void AutoCheck1_Clicked(object sender, EventArgs e) {

        }

        private void AutoCheck2_Clicked(object sender, EventArgs e) {

        }

        private void AutoCheck3_Clicked(object sender, EventArgs e) {

        }

        private void NetSet1_Clicked(object sender, EventArgs e) {

        }

        private void NetSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {

        }

        private void NetSet2_Clicked(object sender, EventArgs e) {

        }

        private void BrightnessSlider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {

            BrightnessText.Text = ((int)((Slider)sender).Value).ToString();

            SetBrightnesstAsync();

        }
        private void AutoSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void RandomSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }
    }
}