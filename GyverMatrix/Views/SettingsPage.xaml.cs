using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage {
        public SettingsPage() =>
            InitializeComponent();

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

        private void BrightnessSlider_OnValueChanged(object sender, ValueChangedEventArgs e) =>
            BrightnessText.Text = ((int)((Slider)sender).Value).ToString();
    }
}