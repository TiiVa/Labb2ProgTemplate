namespace Labb2ProgTemplate;

public class CurrencyHandler
{
    public Dictionary<string, double> PriceForDifferentCurrencies = new Dictionary<string, double>();

    public CurrencyHandler()
    {

        PriceForDifferentCurrencies["SEK"] = 1;
        PriceForDifferentCurrencies["EUR"] = 0.09;
        PriceForDifferentCurrencies["GBP"] = 0.07;

    }


}