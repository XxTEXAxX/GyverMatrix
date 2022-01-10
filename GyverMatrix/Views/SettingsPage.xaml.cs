using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GyverMatrix.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            

        }

        private void BrightnessChanged(object sender, ValueChangedEventArgs e)
        {
            int value = (int) e.NewValue;
            BrightnessText.Text = value.ToString();
        }

        private void DemoSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void Undo_Clicked(object sender, EventArgs e)
        {

        }

        private void Next_Clicked(object sender, EventArgs e)
        {

        }

        private void AutoCheck1_Clicked(object sender, EventArgs e)
        {

        }

        private void AutoCheck2_Clicked(object sender, EventArgs e)
        {

        }

        private void AutoCheck3_Clicked(object sender, EventArgs e)
        {

        }

        private void NetSet1_Clicked(object sender, EventArgs e)
        {

        }

        private void NetSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void NetSet2_Clicked(object sender, EventArgs e)
        {

        }
    }
}