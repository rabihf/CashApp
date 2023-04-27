using System.Collections.Generic;
using System.Linq;

namespace HelperLibrary
{
    public class Cashes 
    {
        public IEnumerable<CurrencyEnum> Currencies => typeof(CurrencyEnum).GetEnumValues().Cast<CurrencyEnum>();

        internal static readonly Dictionary<CurrencyEnum, Cash> CashDict = new Dictionary<CurrencyEnum, Cash>();
        // public readonly List<decimal> CashesAmount = new List<decimal>(); // = CashDict.Aggregate(amt, values) => amt + values.Value;

        public decimal CashLBPAmount => CashDict.TryGetValue(CurrencyEnum.LBP, out _cashLBP) ? _cashLBP.Amount : 0;
        public decimal CashUSDAmount => CashDict.TryGetValue(CurrencyEnum.USD, out _cashUSD) ? _cashUSD.Amount : 0;
        private Cash _cashLBP = new Cash(CurrencyEnum.LBP, 0); //  { get; set; }

        private Cash _cashUSD = new Cash(CurrencyEnum.USD, 0); // { get; set; }
        
        public Cashes()
        {
            // foreach (CurrencyEnum currency in Currencies)
            // {
            //     CashDict[currency] = new Cash(currency, 0);
            // }
        }

        public static void Add(Cash a)
        {
            if (CashDict.ContainsKey(a.CurrEnum))
            {
                var newCash = new Cash(a.CurrEnum,
                    a.QtyHundred + CashDict[a.CurrEnum].QtyHundred,
                    a.QtyFifty + CashDict[a.CurrEnum].QtyFifty,
                    a.QtyTwenty + CashDict[a.CurrEnum].QtyTwenty,
                    a.QtyTen + CashDict[a.CurrEnum].QtyTen,
                    a.QtyFive + CashDict[a.CurrEnum].QtyFive,
                    a.QtyOne + CashDict[a.CurrEnum].QtyOne);
                CashDict[a.CurrEnum] = newCash;
            }
            else
            {
                CashDict[a.CurrEnum] = a; 
            }
        }

        public override string ToString()
        {
            var output = CashDict.Aggregate(string.Empty, (current, keyValuePair) => current + keyValuePair.Value);
            output += "\nTOTAL: ";
            output += $"{CashLBPAmount:N0} LBP";
            output += " - ";
            output += $"{CashUSDAmount:N2} USD";
            return output;
        }
    }
}