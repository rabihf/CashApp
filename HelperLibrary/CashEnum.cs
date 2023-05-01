using System.ComponentModel;

namespace HelperLibrary
{
    public enum CashEnum
    {
        
    }
    public enum CashLBPEnum
    {
        HundredThousand = 100_000,
        FiftyThousand = 50_000,
        TwentyThousand = 20_000,
        TenThousand = 10_000,
        FiveThousand = 5_000,
        OneThousand = 1_000
    }
    public enum CashUSDEnum
    {
        Hundred = 100,
        Fifty = 50,
        Twenty = 20,
        Ten = 10,
        Five = 5,
        One = 1
    }
    public enum CurrencyEnum
    {
        [Description("USD")] USD = 97_000,
        [Description("LBP")] LBP = 1
    }


}