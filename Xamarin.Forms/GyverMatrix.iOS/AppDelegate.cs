namespace GyverMatrix.iOS;

[Register("AppDelegate")]
public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
{
    public override bool FinishedLaunching(
        UIApplication app, 
        NSDictionary options)
    {
        Xamarin.Forms.Forms.Init();
        ColorPickerEffects.Init();
        var _ = new TouchTracking.Forms.iOS.TouchEffect(); SegmentedControlRenderer.Init();
        LoadApplication(new App());

        return base.FinishedLaunching(app, options);
    }
}