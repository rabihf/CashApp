using System.ComponentModel;
using static HelperLibrary.Cash.CurrencyEnum;

namespace HelperLibrary
{
    public class Cash
    {
        private readonly int[] _billsLBP = { 100_000, 50_000, 20_000, 10_000, 5_000, 1_000 };
        private readonly int[] _billsUSD = { 100, 50, 20, 10, 5, 1 };
        
        private const CurrencyEnum DefaultCurrency = LBP;
        private const string DefaultStrFormat = "{0,12:N0} {1,-3}";

        private double ExchangeRate { get; set; } = 1;
        private double Amount { get; set; }
        // private decimal Amount => numericUpDown100.Value * decimal.Parse(button100.Text.Trim(',')) +
        //                           numericUpDown50.Value * decimal.Parse(button50.Text.Trim(',')) +
        //                           numericUpDown20.Value * decimal.Parse(button20.Text.Trim(',')) +
        //                           numericUpDown10.Value * decimal.Parse(button10.Text.Trim(',')) +
        //                           numericUpDown5.Value * decimal.Parse(button5.Text.Trim(',')) +
        //                           numericUpDown1.Value * decimal.Parse(button1.Text.Trim(','));

        private double AmountToBase => Amount * ExchangeRate;

        private string Currency { get; set; } = DefaultCurrency.ToString();
        private string StrFormat { get; set; } = DefaultStrFormat;

        public Cash(CurrencyEnum currency, int hundred, int fifty, int twenty, int ten, int five, int one)
        {
            switch (currency)
            {
                case USD:
                    StrFormat = "{0,12:N2} {1,-3}";
                    ExchangeRate = (double)currency;
                    Currency = currency.ToString();
                    Amount = hundred * (int)CashUSDEnum.Hundred +
                             fifty * (int)CashUSDEnum.Fifty +
                             twenty * (int)CashUSDEnum.Twenty +
                             ten * (int)CashUSDEnum.Ten +
                             five * (int)CashUSDEnum.Five +
                             one * (int)CashUSDEnum.One;
                    break;
                case LBP:
                    ExchangeRate = (double)currency;
                    Amount = hundred * (int)CashLBPEnum.HundredThousand +
                             fifty * (int)CashLBPEnum.FiftyThousand +
                             twenty * (int)CashLBPEnum.TwentyThousand +
                             ten * (int)CashLBPEnum.TenThousand +
                             five * (int)CashLBPEnum.FiveThousand +
                             one * (int)CashLBPEnum.OneThousand;
                    break;
            }
        }

        private Cash()
        {
        }

        public override string ToString()
        {
            return Currency == LBP.ToString()
                ? string.Format(StrFormat, Amount, Currency)
                : string.Format(StrFormat, Amount, Currency) + " ~ " +
                  string.Format(DefaultStrFormat, Amount * ExchangeRate, DefaultCurrency);
        }

        public static Cash operator +(Cash a, Cash b) // overloading
        {
            var cash = new Cash();
            if (a.Currency == b.Currency)
            {
                cash.Currency = a.Currency;
                cash.Amount = a.Amount + b.Amount;
                cash.ExchangeRate = a.ExchangeRate;
            }
            else
            {
                cash.Amount = a.AmountToBase * a.ExchangeRate + b.AmountToBase * b.ExchangeRate;
            }
            return cash;
        }

        public static Cash operator -(Cash a, Cash b) // overloading
        {
            var cash = new Cash();
            if (a.Currency == b.Currency)
            {
                cash.Currency = a.Currency;
                cash.Amount = a.Amount - b.Amount;
                cash.ExchangeRate = a.ExchangeRate;
            }
            else
            {
                cash.Amount = a.AmountToBase * a.ExchangeRate - b.AmountToBase * b.ExchangeRate;
            }
            return cash;
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

        private enum CashUSDEnum
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
            [Description("USD")] USD = 110_000,
            [Description("LBP")] LBP = 1
        }
    }
}