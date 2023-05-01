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

        public void Subtract(Cash cashA)
        {
            var qtyOne = CashDict[cashA.CurrEnum].QtyOne;
            var qtyFive = CashDict[cashA.CurrEnum].QtyFive;
            var qtyTen = CashDict[cashA.CurrEnum].QtyTen;
            var qtyTwenty = CashDict[cashA.CurrEnum].QtyTwenty;
            var qtyFifty = CashDict[cashA.CurrEnum].QtyFifty;
            var qtyHundred = CashDict[cashA.CurrEnum].QtyHundred;
            if (CashDict.ContainsKey(cashA.CurrEnum))
            {
                if (CashDict[cashA.CurrEnum].Amount >= cashA.Amount)
                {
                    // check qtyOne
                    if (qtyOne < cashA.QtyOne)
                    {
                        if (qtyFive > 0) // 5 => (5 * 1)
                        {
                            qtyFive -= 1; // subtract 1 from 5 bill
                            qtyOne += 5; // Add 5 to 1 bill
                            qtyOne -= cashA.QtyOne;
                        }
                        else if (qtyTen > 0) // 10 => (1 * 5) + (5 * 1)
                        {
                            qtyTen -= 1; // subtract 1 from 10 bill
                            qtyFive += 1; // Add 1 to 5 bill
                            qtyOne += 5; // Add 5 to 1 bill
                            qtyOne -= cashA.QtyOne;
                        }
                        else if (qtyTwenty > 0) // 20 => (1 * 10) + (1 * 5) + (5 * 1)
                        {
                            qtyTwenty -= 1; // Subtract 1 from 20 bill
                            qtyTen += 1; // Add 1 to 10 bill
                            qtyFive += 1; // Add 1 to 5 bill
                            qtyOne += 5; // Add 5 to 1 bill
                            qtyOne -= cashA.QtyOne;
                        }
                        else if (qtyFifty > 0) // 50 => (2 * 20) + (0 * 10) + (1 * 5) + (5 * 1)
                        {
                            qtyFifty -= 1; // Subtract 1 from 50 bill
                            qtyTwenty += 2; // Add 2 to 20 bill
                            qtyFive += 1; // Add 1 to 5 bill
                            qtyOne += 5; // Add 5 to 1 bill
                            qtyOne -= cashA.QtyOne;
                        }
                        else if (qtyHundred > 0) // 100 => (1 * 50) + (2 * 20) + (0 * 10) + (1 * 5) + (5 * 1)
                        {
                            qtyHundred -= 1; // Subtract 1 from 100 bill
                            qtyFifty += 1; // Add 1 to 50 bill
                            qtyTwenty += 2; // Add 2 to 20 bill
                            qtyFive += 1; // Add 1 to 5 bill
                            qtyOne = +5; // Add 5 to 1 bill
                            qtyOne -= cashA.QtyOne;
                        }
                    }
                    else
                    {
                        qtyOne -= cashA.QtyOne;
                    }

                    // check qtyFive
                    if (qtyFive < cashA.QtyFive)
                    {
                        if (qtyTen > 0) // 10 => (2 * 5)
                        {
                            qtyTen -= 1; // Subtract 1 from 10 bill
                            qtyFive += 2; // Add 2 to 5 bill
                            qtyFive -= cashA.QtyFive;
                        }
                        else if (qtyTwenty > 0) // 20 => (1 * 10 ) + (2 * 5)
                        {
                            qtyTwenty -= 1; // Subtract 1 from 20 bill
                            qtyTen += 1; // Add 1 to 10 bill
                            qtyFive += 2; // Add 2 to 5 bill
                            qtyFive -= cashA.QtyFive;
                        }
                        else if (qtyFifty > 0) // 50 => (2 * 20) + (2 * 5)
                        {
                            qtyFifty -= 1; // Subtract 1 from 50 bill
                            qtyTwenty += 2; // Add 2 to 20 bill
                            qtyFive += 2; // Add 2 to 5 bill
                            qtyFive -= cashA.QtyFive;
                        }
                        else if (qtyHundred > 0) // 100 => (1 * 50) + (2 * 20) + (2 * 5)
                        {
                            qtyHundred -= 1; // Subtract 1 from 100 bill
                            qtyFifty += 1; // Add 1 to 50 bill
                            qtyTwenty += 2; // Add 2 to 20 bill
                            qtyFive += 2; // Add 2 t 5 bill
                            qtyFive -= cashA.QtyFive;
                        }
                    }
                    else
                    {
                        qtyFive -= cashA.QtyFive;
                    }

                    // Check qtyTen
                    if (qtyTen < cashA.QtyTen)
                    {
                        if (qtyTwenty > 0) // 20 => (2 * 10)
                        {
                            qtyTwenty -= 1; // Subtract 1 from 20 bill
                            qtyTen += 2; // Add 2 to 10 bill
                            qtyTen -= cashA.QtyTen;
                        }
                        else if (qtyFifty > 0) // 50 => (2 * 20) + (1 * 10)
                        {
                            qtyFifty -= 1; // Subtract 1 from 50 bill
                            qtyTwenty += 2; // Add 2 to 20 bill
                            qtyTen += 1; // Add 1 to 10 bill
                            qtyTen -= cashA.QtyTen;
                        }
                        else if (qtyHundred > 0) // 100 => (1 * 50) + (2 * 20) + (1* 10)
                        {
                            qtyHundred -= 1; // Subtract 1 from 100 bill
                            qtyFifty += 1; // Add 1 to 50 bill
                            qtyTwenty += 2; // Add 2 to 20 bill
                            qtyTen += 1; // Add 1 to 10 bill
                            qtyTen -= cashA.QtyTen;
                        }
                    }
                    else
                    {
                        qtyTen -= cashA.QtyTen;
                    }

                    // Check qtyTwenty
                    if (qtyTwenty < cashA.QtyTwenty)
                    {
                        if (qtyFifty > 0) // 50 => (2 * 20) + (1 * 10)
                        {
                            qtyFifty -= 1; // Subtract 1 fro 50 bill
                            qtyTwenty += 2; // Add 2 to 20 bill
                            qtyTen += 1; // Add 1 to 10 bill
                            qtyTwenty -= cashA.QtyTwenty;
                        }
                        else if (qtyHundred > 0) // 100 => (1 * 50) + (2 * 20) + (1 * 10)
                        {
                            qtyHundred -= 1; // Subtract 1 from 100 bill
                            qtyFifty += 1; // Add 1 to 50 bill
                            qtyTwenty += 2; // Add 2 to 20 bill
                            qtyTen += 1; // Add 1 to 10 bill
                            qtyTwenty -= cashA.QtyTwenty;
                        }
                    }
                    else
                    {
                        qtyTwenty -= cashA.QtyTwenty;
                    }

                    // Check qtyFifty
                    if (qtyFifty < cashA.QtyFifty)
                    {
                        if (qtyHundred > 0) // 100 => (2 * 50)
                        {
                            qtyHundred -= 1; // Subtract 1 from 100 bill
                            qtyFifty += 2; // Add 2 to 50 bill
                            qtyFifty -= cashA.QtyFifty;
                        }
                    }
                    else
                    {
                        qtyFifty -= cashA.QtyFifty;
                    }

                    qtyHundred -= cashA.QtyHundred;

                    var newCash = new Cash(cashA.CurrEnum, qtyHundred, qtyFifty, qtyTwenty, qtyTen, qtyFive, qtyOne);
                    CashDict[cashA.CurrEnum] = newCash;
                }
                else
                {
                    throw new Exception("The Cash Amount could not be subtracted");
                }
            }
            else // CashDict doesnt contains Currency (cashA.CurrEnum)
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