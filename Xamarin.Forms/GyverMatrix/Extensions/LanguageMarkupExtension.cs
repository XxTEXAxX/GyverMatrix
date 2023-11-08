namespace GyverMatrix.Extensions;

[ContentProperty("Text")]
public class LanguageMarkupExtension : IMarkupExtension
{
    private readonly CultureInfo _cultureInfo;

    private const string ResourceId = "GyverMatrix.Languages.Resource";

    public string Text { get; set; }

    public LanguageMarkupExtension()
    {
        _cultureInfo = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
    }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        if (Text == null)
            return string.Empty;

        ResourceManager resmgr = new(ResourceId,
                    typeof(LanguageMarkupExtension).GetTypeInfo().Assembly);

        var translation = resmgr.GetString(Text, _cultureInfo);

        translation ??= Text;
        return translation;
    }
}