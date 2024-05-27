using TranslitSharp;

namespace BeautySaloon.Helpers;

public static class GenerateUserName
{
    public static string Generate(string firstName, string secondName)
    {
        var translit = new Transliterator(configuration => configuration.ConfigureCyrillicToLatin());
        var transliteratedFirstName = translit.Transliterate(firstName);
        var transliteratedSecondName = translit.Transliterate(secondName);

        var baseUserName = $"{transliteratedFirstName}{transliteratedSecondName}".ToLower();
        var randomSuffix = new Random().Next(1000, 9999);
        return $"{baseUserName}{randomSuffix}";
    }
}