﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GyverMatrix.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EffectsPage : ContentPage
    {
        public EffectsPage()
        {
            InitializeComponent();
        }

        private void EffectSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void HourSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void BrightnessSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            BrightnessText.Text = ((int)((Slider)sender).Value).ToString();
        }

        private void SpeedSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SpeedText.Text = ((int)((Slider)sender).Value).ToString();
        }
    }
}