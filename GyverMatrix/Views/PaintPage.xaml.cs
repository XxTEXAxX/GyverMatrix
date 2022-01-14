using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaintPage {
        public PaintPage() =>
            InitializeComponent();

        private void PaintPage_OnAppearing(object sender, EventArgs e) {
            int h = 14, w = 21;
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
        }
    }
}