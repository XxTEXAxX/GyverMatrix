using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaintPage {
        public PaintPage() =>
            InitializeComponent();

        private async void PaintPage_OnAppearing(object sender, EventArgs e) {
            int h = int.Parse(await SecureStorage.GetAsync("H"));
            int w = int.Parse(await SecureStorage.GetAsync("W"));

            int ScrW = (int) DeviceDisplay.MainDisplayInfo.Width;
            int padding = 10;
            Console.WriteLine(ScrW);
            ScrW = ScrW - padding * 2;
            Console.WriteLine(ScrW);

            double size = (ScrW / w / 2.5);
            Console.WriteLine(size);
            Console.WriteLine(size * w);
            for (int i = 0; i < w ; i++)
            {

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(size) });

            }

            for (int i = 0; i < h ; i++)
            {

                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(size) });

            }

            for (int r = 0; r < h; r++)
            {
                for (int c = 0; c < w; c++)
                {
                    Button btn = new Button();
                    //btn.WidthRequest = size;
                    //btn.HeightRequest = size;
                    btn.Text = "H";
                    btn.BackgroundColor = Color.Chartreuse;
                    btn.SetValue(Grid.RowProperty, r);
                    btn.SetValue(Grid.ColumnProperty, c);
                    grid.Children.Add(btn);
                }
            }

            /*
            for (int i = 0; i < w; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < h; i++)
                grid.RowDefinitions.Add(new RowDefinition());
            for (int r = 0; r < h; r++) {
                for (int c = 0; c < w; c++) {
                    Button btn = new Button();
                    btn.WidthRequest = 30;
                    btn.HeightRequest = 30;
                    btn.Text = "H";
                    btn.BackgroundColor = Color.Chartreuse;
                    btn.SetValue(Grid.RowProperty, r);
                    btn.SetValue(Grid.ColumnProperty, c);
                    grid.Children.Add(btn);
                }
            }
            */
        }
    }
}