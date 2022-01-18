using System;
using System.Collections.Generic;
using SkiaSharp;
using TouchTracking;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaintPage {
        Dictionary<long, SKPath> inProgressPaths = new Dictionary<long, SKPath>();
        List<SKPath> completedPaths = new List<SKPath>();

        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Blue,
            StrokeWidth = 10,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };
        public PaintPage() =>
            InitializeComponent();

        private async void PaintPage_OnAppearing(object sender, EventArgs e) {
            //int h = int.Parse(await SecureStorage.GetAsync("H"));
            //int w = int.Parse(await SecureStorage.GetAsync("W"));

            //int ScrW = (int) DeviceDisplay.MainDisplayInfo.Width;
            //const int padding = 10;
            //Console.WriteLine(ScrW);
            //ScrW = ScrW - padding * 2;
            //Console.WriteLine(ScrW);

            //double size = (ScrW / w / 2.5);
            //Console.WriteLine(size);
            //Console.WriteLine(size * w);
            //for (int i = 0; i < w ; i++)
            //{
            //    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(size) });
            //}

            //for (int i = 0; i < h ; i++)
            //{
            //    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(size) });
            //}

            //for (int r = 0; r < h; r++)
            //{
            //    for (int c = 0; c < w; c++)
            //    {
            //        Button btn = new Button
            //        {
            //            //btn.HeightRequest = size;
            //            //btn.WidthRequest = size;
            //            Text = "H",
            //            BackgroundColor = Color.Chartreuse
            //        };
            //        btn.SetValue(Grid.RowProperty, r);
            //        btn.SetValue(Grid.ColumnProperty, c);
            //        grid.Children.Add(btn);
            //    }
            //}

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

        private void canvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e) {
            //Lab.Text = e.Info.Size.Width.ToString() + " | " + e.Info.Size.Height.ToString();
            SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear();

            foreach (SKPath path in completedPaths)
            {
                canvas.DrawPath(path, paint);
            }

            foreach (SKPath path in inProgressPaths.Values)
            {
                canvas.DrawPath(path, paint);
            }
        }

        private void TouchEffect_TouchAction(object sender, TouchTracking.TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!inProgressPaths.ContainsKey(args.Id))
                    {
                        SKPath path = new SKPath();
                        path.MoveTo(ConvertToPixel(args.Location));
                        inProgressPaths.Add(args.Id, path);
                        canvasView.InvalidateSurface();
                    }

                    break;

                case TouchActionType.Moved:
                    if (inProgressPaths.ContainsKey(args.Id))
                    {
                        SKPath path = inProgressPaths[args.Id];
                        path.LineTo(ConvertToPixel(args.Location));
                        canvasView.InvalidateSurface();
                    }

                    break;

                case TouchActionType.Released:
                    if (inProgressPaths.ContainsKey(args.Id))
                    {
                        completedPaths.Add(inProgressPaths[args.Id]);
                        inProgressPaths.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }

                    break;

                case TouchActionType.Cancelled:
                    if (inProgressPaths.ContainsKey(args.Id))
                    {
                        inProgressPaths.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }

                    break;
            }
        }
        SKPoint ConvertToPixel(TouchTrackingPoint pt)
        {
            return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
        }
    }
}