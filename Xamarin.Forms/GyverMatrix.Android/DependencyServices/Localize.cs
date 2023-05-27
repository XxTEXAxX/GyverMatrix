[assembly: Dependency(typeof(GyverMatrix.Droid.DependencyServices.Localize))]
namespace GyverMatrix.Droid.DependencyServices;

public class Localize : ILocalize
{
    public CultureInfo GetCurrentCultureInfo()
    {
        var androidLocale = Locale.Default;
        var netLanguage = androidLocale.ToString().Replace("_", "-");
        return new CultureInfo(netLanguage);
    }
}