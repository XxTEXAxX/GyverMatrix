using System;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaintPage {
        //Dictionary<long, SKPath> inProgressPaths = new Dictionary<long, SKPath>();
        //List<SKPath> completedPaths = new List<SKPath>();
        //SKPaint paint = new SKPaint {
        //    Style = SKPaintStyle.Stroke,
        //    Color = SKColors.Blue,
        //    StrokeWidth = 10,
        //    StrokeCap = SKStrokeCap.Round,
        //    StrokeJoin = SKStrokeJoin.Round
        //};
        enum PaintModes {
            Brush, Erase
        }
        private Frame[,] _frames;
        private double _size;
        private Color _currentColor = Color.DarkOrange;

        private PaintModes _currentMode = PaintModes.Brush;
        //int h = int.Parse(await SecureStorage.GetAsync("H"));
        //int w = int.Parse(await SecureStorage.GetAsync("W"));
        int h = 21, w = 16;
        public PaintPage() =>
            InitializeComponent();

        private void PaintPage_OnAppearing(object sender, EventArgs e) {
            _size = (Application.Current.MainPage.Width / w / 1.1);
            for (int i = 0; i < w; i++) {
                CustomGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(_size) });
            }

            for (int i = 0; i < h; i++) {
                CustomGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(_size) });
            }

            _frames = new Frame[h, w];
            for (int r = 0; r < h; r++) {
                for (int c = 0; c < w; c++) {
                    Frame btn = new Frame {
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

        private void TouchEffect_TouchAction(object sender, TouchTracking.TouchActionEventArgs args) {
            try {
                if (!(args.Location.X > 0) || !(args.Location.Y > 0) || !(args.Location.Y < CustomGrid.Height) || !(args.Location.X < CustomGrid.Width))
                    return;
                var column = (int)(args.Location.X / _size);
                var row = (int)(args.Location.Y / _size);
                _frames[row, column].BackgroundColor = _currentMode switch {
                    PaintModes.Brush => _currentColor,
                    _ => Color.Transparent
                };
            } catch {
                // ignored
            }
        }

        private void Bucket_Clicked(object sender, EventArgs e) {
            for (int r = 0; r < h; r++) {
                for (int c = 0; c < w; c++) {
                    _frames[r, c].BackgroundColor = _currentColor;
                }
            }
        }

        private void Erase_Clicked(object sender, EventArgs e) {
            Erase.BackgroundColor = Color.Green;
            Brush.BackgroundColor = Color.Transparent;
            _currentMode = PaintModes.Erase;
        }

        private void Brush_Clicked(object sender, EventArgs e) {
            Erase.BackgroundColor = Color.Transparent;
            Brush.BackgroundColor = Color.Green;
            _currentMode = PaintModes.Brush;
        }

        private void Clear_Clicked(object sender, EventArgs e) {
            for (int r = 0; r < h; r++) {
                for (int c = 0; c < w; c++) {
                    _frames[r, c].BackgroundColor = Color.Transparent;
                }
            }
        }
    }
}