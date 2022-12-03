using ColorPicker;
using GyverMatrix.Helpers;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GyverMatrix.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaintPage : INotifyPropertyChanged
    {
        enum PaintModes
        {
            Brush, Erase
        }
        private Frame[,] _frames;
        private double _size;
        public Color CurrentColor { get; set; } = Color.DarkOrange;

        private PaintModes _currentMode = PaintModes.Brush;
        public PaintPage()
        {
            InitializeComponent();


        }

        int _h = 16;
        int _w = 16;

        private async void PaintPage_OnAppearing(object sender, EventArgs e)
        {
            try
            {
                _h = int.Parse(await SecureStorage.GetAsync("H"));
                _w = int.Parse(await SecureStorage.GetAsync("W"));
            }
            catch
            {

            }
            int.TryParse(await SecureStorage.GetAsync("BR"), out var result);
            BrightnessSlider.Value = result;

            _size = (Application.Current.MainPage.Width / _w / 1.1);
            for (int i = 0; i < _w; i++)
            {
                CustomGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(_size) });
            }

            for (int i = 0; i < _h; i++)
            {
                CustomGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(_size) });
            }

            _frames = new Frame[_h, _w];
            for (int r = 0; r < _h; r++)
            {
                for (int c = 0; c < _w; c++)
                {
                    Frame btn = new Frame
                    {
                        BorderColor = Color.DarkOrange,
                        BackgroundColor = Color.Transparent,
                        CornerRadius = 0
                    };
                    _frames[r, c] = btn;
                    btn.SetValue(Grid.RowProperty, r);
                    btn.SetValue(Grid.ColumnProperty, c);
                    CustomGrid.Children.Add(btn);
                }
            }
        }

        private async void TouchEffect_TouchAction(object sender, TouchTracking.TouchActionEventArgs args)
        {
            try
            {
                if (!(args.Location.X > 0) || !(args.Location.Y > 0) || !(args.Location.Y < CustomGrid.Height) || !(args.Location.X < CustomGrid.Width))
                    return;
                var column = (int)Math.Ceiling(args.Location.X / _size) - 1;
                var row = (int)Math.Ceiling(args.Location.Y / _size) - 1;

                var x = column;
                var y = _h - row - 1;

                Console.WriteLine(x + " " + y);
                await UdpHelper.Send("$1 " + x + " " + y + ";");
                _frames[row, column].BackgroundColor = _currentMode switch
                {
                    PaintModes.Brush => CurrentColor,
                    _ => Color.Transparent
                };
            }
            catch
            {
                // ignored
            }
        }

        private async void Bucket_Clicked(object sender, EventArgs e)
        {
            await UdpHelper.Send("$2;");
            for (int r = 0; r < _h; r++)
            {
                for (int c = 0; c < _w; c++)
                {


                    _frames[r, c].BackgroundColor = CurrentColor;
                }
            }
        }

        private async void Erase_Clicked(object sender, EventArgs e)
        {
            Erase.BackgroundColor = Color.Green;
            Brush.BackgroundColor = Color.Transparent;
            _currentMode = PaintModes.Erase;
            await UdpHelper.Send("$0 000000;");
        }

        private async void Brush_Clicked(object sender, EventArgs e)
        {
            Erase.BackgroundColor = Color.Transparent;
            Brush.BackgroundColor = Color.Green;
            _currentMode = PaintModes.Brush;
            await UdpHelper.Send("$0 " + CurrentColor.ToHex().Remove(0, 3) + ";");
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            PickerBlock.IsVisible = true;
            GridBlock.IsVisible = false;
        }

        private async void CloseColorPicker_Clicked(object sender, EventArgs e)
        {
            string col = _currentMode switch
            {
                PaintModes.Brush => "$0 " + CurrentColor.ToHex().Remove(0, 3) + ";",
                _ => "$0 000000;"
            };

            await UdpHelper.Send(col);

            PickerBlock.IsVisible = false;
            GridBlock.IsVisible = true;
        }

        private async void Clear_Clicked(object sender, EventArgs e)
        {

            await UdpHelper.Send("$3;");

            for (int r = 0; r < _h; r++)
            {
                for (int c = 0; c < _w; c++)
                {
                    _frames[r, c].BackgroundColor = Color.Transparent;
                }
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        private void ColorTriangle_SelectedColorChanged(object sender, ColorPicker.BaseClasses.ColorPickerEventArgs.ColorChangedEventArgs e)
        {
            ColorTriangle colorPicker = (ColorTriangle)sender;
            CurrentColor = colorPicker.SelectedColor;
            NotifyPropertyChanged(nameof(CurrentColor));
        }

        private async Task SetBrightnesstAsync()
        {
            await UdpHelper.Send("$4 0 " + (int)BrightnessSlider.Value + ";");
            await SecureStorage.SetAsync("BR", ((int)BrightnessSlider.Value).ToString());
        }

        private async void BrightnessSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            BrightnessText.Text = ((int)((Slider)sender).Value).ToString();
            await SetBrightnesstAsync();
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}