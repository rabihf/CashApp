using System;
using System.Collections.Generic;
using System.Linq;
using static HelperLibrary.CurrencyEnum;

namespace HelperLibrary
{
    public class Cashes
    {
        public IEnumerable<CurrencyEnum> Currencies => typeof(CurrencyEnum).GetEnumValues().Cast<CurrencyEnum>();

        private Dictionary<CurrencyEnum, Cash> CashDict { get; set; }

        private const string USDStrFormat = @"{0:N2}";
        private const string LBPStrFormat = @"{0:N0}";
        public string CashLBPAmountString => string.Format(LBPStrFormat, CashLBPAmount) + @" " + LBP;
        public string CashUSDAmountString => string.Format(USDStrFormat, CashUSDAmount) + @" " + USD;


        private decimal CashLBPAmount => CashDict.TryGetValue(LBP, out CashLBP) ? CashLBP.Amount : 0;
        private decimal CashUSDAmount => CashDict.TryGetValue(USD, out CashUSD) ? CashUSD.Amount : 0;
        public Cash CashLBP;

        public Cash CashUSD;

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

        public void Subtract(Cash cashA)
        {
            var qtyOne = CashDict[cashA.CurrEnum].QtyOne;
            var qtyFive = CashDict[cashA.CurrEnum].QtyFive;
            var qtyTen = CashDict[cashA.CurrEnum].QtyTen;
            var qtyTwenty = CashDict[cashA.CurrEnum].QtyTwenty;
            var qtyFifty = CashDict[cashA.CurrEnum].QtyFifty;
            var qtyHundred = CashDict[cashA.CurrEnum].QtyHundred;
            var cashD = new Cash(cashA.CurrEnum, 0);
            if (CashDict.ContainsKey(cashA.CurrEnum))
            {
                if (CashDict[cashA.CurrEnum].Amount >= cashA.Amount)
                {
                    // check qtyOne
                    if (qtyOne < cashA.QtyOne)
                    {
                        if (qtyFive > 0) // 5 => (5 * 1)
                        {
                            cashD += new Cash(cashA.CurrEnum, 0, 0, 0, 0, -1, 5);
                        }
                        else if (qtyTen > 0) // 10 => (1 * 5) + (5 * 1)
                        {
                            cashD += new Cash(cashA.CurrEnum, 0, 0, 0, -1, 1, 5);
                        }
                        else if (qtyTwenty > 0) // 20 => (1 * 10) + (1 * 5) + (5 * 1)
                        {
                            cashD += new Cash(cashA.CurrEnum, 0, 0, -1, 1, 1, 5);
                        }
                        else if (qtyFifty > 0) // 50 => (2 * 20) + (0 * 10) + (1 * 5) + (5 * 1)
                        {
                            cashD += new Cash(cashA.CurrEnum, 0, -1, 2, 0, 1, 5);
                        }
                        else if (qtyHundred > 0) // 100 => (1 * 50) + (2 * 20) + (0 * 10) + (1 * 5) + (5 * 1)
                        {
                            cashD += new Cash(cashA.CurrEnum, -1, 1, 2, 0, 1, 5);
                        }
                    }

                    cashD += new Cash(cashA.CurrEnum, 0, 0, 0, 0, 0, -cashA.QtyOne);

                    // check qtyFive
                    if (qtyFive < cashA.QtyFive)
                    {
                        if (qtyTen > 0) // 10 => (2 * 5)
                        {
                            cashD += new Cash(cashA.CurrEnum, 0, 0, 0, -1, 2, 0);
                        }
                        else if (qtyTwenty > 0) // 20 => (1 * 10 ) + (2 * 5)
                        {
                            cashD += new Cash(cashA.CurrEnum, 0, 0, -1, 1, 2, 0);
                        }
                        else if (qtyFifty > 0) // 50 => (2 * 20) + (2 * 5)
                        {
                            cashD += new Cash(cashA.CurrEnum, 0, -1, 2, 0, 2, 0);
                        }
                        else if (qtyHundred > 0) // 100 => (1 * 50) + (2 * 20) + (2 * 5)
                        {
                            cashD += new Cash(cashA.CurrEnum, -1, 1, 2, 0, 2, 0);
                        }
                    }

                    cashD += new Cash(cashA.CurrEnum, 0, 0, 0, 0, -cashA.QtyFive, 0);

                    // Check qtyTen
                    if (qtyTen < cashA.QtyTen)
                    {
                        if (qtyTwenty > 0) // 20 => (2 * 10)
                        {
                            cashD += new Cash(cashA.CurrEnum, 0, 0, -1, 2, 0, 0);
                        }
                        else if (qtyFifty > 0) // 50 => (2 * 20) + (1 * 10)
                        {
                            cashD += new Cash(cashA.CurrEnum, 0, -1, 2, 1, 0, 0);
                        }
                        else if (qtyHundred > 0) // 100 => (1 * 50) + (2 * 20) + (1* 10)
                        {
                            cashD += new Cash(cashA.CurrEnum, -1, 1, 2, 1, 0, 0);
                        }
                    }

                    cashD += new Cash(cashA.CurrEnum, 0, 0, 0, -cashA.QtyTen, 0, 0);

                    // Check qtyTwenty
                    if (qtyTwenty < cashA.QtyTwenty)
                    {
                        if (qtyFifty > 0) // 50 => (2 * 20) + (1 * 10)
                        {
                            cashD += new Cash(cashA.CurrEnum, 0, -1, 2, 1, 0, 0);
                        }
                        else if (qtyHundred > 0) // 100 => (1 * 50) + (2 * 20) + (1 * 10)
                        {
                            cashD += new Cash(cashA.CurrEnum, -1, 1, 2, 1, 0, 0);
                        }
                    }

                    cashD += new Cash(cashA.CurrEnum, 0, 0, -cashA.QtyTwenty, 0, 0, 0);

                    // Check qtyFifty
                    if (qtyFifty < cashA.QtyFifty)
                    {
                        if (qtyHundred > 0) // 100 => (2 * 50)
                        {
                            cashD += new Cash(cashA.CurrEnum, -1, 2, 0, 0, 0, 0);
                        }
                    }

                    cashD += new Cash(cashA.CurrEnum, 0, -cashA.QtyFifty, 0, 0, 0, 0);

                    var newCash = new Cash(cashA.CurrEnum, qtyHundred, qtyFifty, qtyTwenty, qtyTen, qtyFive, qtyOne) + cashD;
                    
                    // Check qtyHundred
                    if (qtyHundred < cashA.QtyHundred)
                    {
                        // TODO: Must check backwards to accept subtraction
                        // 100 => ? 50 + ? 20 + ? 10 + ? 5 + ? 1
                        if (newCash.QtyFifty > 0)
                        {
                            
                        }
                    }
                    cashD += new Cash(cashA.CurrEnum, -cashA.QtyHundred, 0, 0, 0, 0, 0);

                    CashDict[cashA.CurrEnum] = newCash + cashD;
                }
                else
                {
                    throw new Exception("The Cash Amount could not be subtracted");
                }
            }
            else // CashDict doesn't contains Currency (cashA.CurrEnum)
            {
                throw new Exception("This currency doesn't exist");
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