namespace GyverMatrix.Extensions;

[ContentProperty("Text")]
public class LanguageMarkupExtension : IMarkupExtension
{
    readonly CultureInfo ci;
    const string ResourceId = "GyverMatrix.Languages.Resource";

    public LanguageMarkupExtension()
    {
        //ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
        ci = new CultureInfo("en-US");
    }

    public string Text { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        if (Text == null)
            return "";

        ResourceManager resmgr = new ResourceManager(ResourceId,
                    typeof(LanguageMarkupExtension).GetTypeInfo().Assembly);

        var translation = resmgr.GetString(Text, ci);

        if (translation == null)
        {
            translation = Text;
        }
        return translation;
    }
}