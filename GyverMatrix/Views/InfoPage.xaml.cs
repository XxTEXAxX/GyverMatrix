using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage {
        public InfoPage() =>
            InitializeComponent();

        private async void ThemeSwitch_OnToggled(object sender, ToggledEventArgs e) {
            Application.Current.UserAppTheme = ThemeSwitch.IsToggled ? OSAppTheme.Dark : OSAppTheme.Light;
            await SecureStorage.SetAsync("Theme", ThemeSwitch.IsToggled ? "Dark" : "Light");
        }

        private async void InfoPage_OnAppearing(object sender, EventArgs e) {
            ThemeSwitch.IsToggled = await SecureStorage.GetAsync("Theme") == "Dark";
        }
    }
}