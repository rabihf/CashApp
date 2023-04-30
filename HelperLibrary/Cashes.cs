using System.Collections.Generic;
using System.Linq;
using static HelperLibrary.CurrencyEnum;

namespace HelperLibrary
{
    public class Cashes 
    {
        public IEnumerable<CurrencyEnum> Currencies => typeof(CurrencyEnum).GetEnumValues().Cast<CurrencyEnum>();

        private Dictionary<CurrencyEnum, Cash> CashDict { get; set; } // = new Dictionary<CurrencyEnum, Cash>();
        // public readonly List<decimal> CashesAmount = new List<decimal>(); // = CashDict.Aggregate(amt, values) => amt + values.Value;

        private const string USDStrFormat = @"{0:N2}";
        private const string LBPStrFormat = @"{0:N0}";
        public string CashLBPAmountString => string.Format(LBPStrFormat,CashLBPAmount) + @" " + LBP;
        public string CashUSDAmountString => string.Format(USDStrFormat,CashUSDAmount) + @" " + USD;


        private decimal CashLBPAmount => CashDict.TryGetValue(LBP, out _cashLBP) ? _cashLBP.Amount : 0;
        private decimal CashUSDAmount => CashDict.TryGetValue(USD, out _cashUSD) ? _cashUSD.Amount : 0;
        private Cash _cashLBP; 

        private Cash _cashUSD; 
        
        public Cashes()
        {
            CashDict = new Dictionary<CurrencyEnum, Cash>();
            // foreach (CurrencyEnum currency in Currencies)
            // {
            //     CashDict[currency] = new Cash(currency, 0);
            // }
        }

        public void Add(Cashes cashes)
        {
            foreach (var cash in cashes.CashDict.Values)
            {
                Add(cash);
            }
        }

        public void Add(Cash cashA)
        {
            if (CashDict.ContainsKey(cashA.CurrEnum))
            {
                var newCash = new Cash(cashA.CurrEnum,
                    cashA.QtyHundred + CashDict[cashA.CurrEnum].QtyHundred,
                    cashA.QtyFifty + CashDict[cashA.CurrEnum].QtyFifty,
                    cashA.QtyTwenty + CashDict[cashA.CurrEnum].QtyTwenty,
                    cashA.QtyTen + CashDict[cashA.CurrEnum].QtyTen,
                    cashA.QtyFive + CashDict[cashA.CurrEnum].QtyFive,
                    cashA.QtyOne + CashDict[cashA.CurrEnum].QtyOne);
                CashDict[cashA.CurrEnum] = newCash;
            }
            else
            {
                CashDict[cashA.CurrEnum] = cashA; 
            }
        }

        public override string ToString()
        {
            var output = CashDict.Aggregate(string.Empty, (current, keyValuePair) => current + keyValuePair.Value);
            output += "\nTOTAL: ";
            output += CashLBPAmountString; 
            output += " - ";
            output += CashUSDAmountString; 
            return output;
        }
    }
}