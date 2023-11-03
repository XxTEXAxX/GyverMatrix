using System.Globalization;
using Foundation;
using GyverMatrix.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(GyverMatrix.iOS.DependencyServices.Localize))]
namespace GyverMatrix.iOS.DependencyServices;


public class Localize : ILocalize
{
    public CultureInfo GetCurrentCultureInfo()
    {
        var netLanguage = "ru-RU";
        var prefLanguage = "en-US";
        if (NSLocale.PreferredLanguages.Length > 0)
        {
            var pref = NSLocale.PreferredLanguages[0];
            netLanguage = pref.Replace("_", "-"); // заменяет pt_BR на pt-BR
        }
        CultureInfo ci;
        try
        {
            ci = new CultureInfo(netLanguage);
        }
        catch
        {
            ci = new CultureInfo(prefLanguage);
        }
        return ci;
    }
}