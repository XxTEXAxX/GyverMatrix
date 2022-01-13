using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaintPage {
        public PaintPage() =>
            InitializeComponent();

        public int Touch { get; set; }

        private void PanGestureRecognizer_OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            Console.WriteLine($"Pan {Touch}");
        }
    }
}