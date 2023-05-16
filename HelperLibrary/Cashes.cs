using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using static HelperLibrary.Cashes.CashesException.CashesError;
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
            if (!CashDict.ContainsKey(cashA.CurrEnum)) throw new CashesException(CurrencyDoesntExist);

            
            if (CashDict[cashA.CurrEnum].Amount >= cashA.Amount)
            {
                if (isSubtractAble(CashDict[cashA.CurrEnum], cashA))
                {
                    CashDict[cashA.CurrEnum] += new Cash(cashA.CurrEnum, -cashA.QtyHundred, -cashA.QtyFifty,
                        -cashA.QtyTwenty, -cashA.QtyTen, -cashA.QtyFive, -cashA.QtyOne);
                }
                else
                {
                    throw new CashesException(CashAmountCouldNotBeSubtracted);
                }
            }
            else
            {
                throw new CashesException(CashExceedAvailableAmount);
            }
        }

        private void CheckAllQty(Cash cashA)
        {
            var qtyOne = CashDict[cashA.CurrEnum].QtyOne;
            var qtyFive = CashDict[cashA.CurrEnum].QtyFive;
            var qtyTen = CashDict[cashA.CurrEnum].QtyTen;
            var qtyTwenty = CashDict[cashA.CurrEnum].QtyTwenty;
            var qtyFifty = CashDict[cashA.CurrEnum].QtyFifty;
            var qtyHundred = CashDict[cashA.CurrEnum].QtyHundred;
            var myCash = CashDict[cashA.CurrEnum];
            var cashDifference = new Cash(cashA.CurrEnum, 0);
            // TODO: Must check this function for qtyOne > 5
            var solvedCashForOne = SolvedCashForOne(myCash, cashA);
            Console.WriteLine($"solvedCashForOne: {solvedCashForOne}");

            // check qtyFive
            if (qtyFive < cashA.QtyFive)
            {
                if (qtyTen > 0) // 10 => (2 * 5)
                {
                    cashDifference += new Cash(cashA.CurrEnum, 0, 0, 0, -1, 2, 0);
                }
                else if (qtyTwenty > 0) // 20 => (1 * 10 ) + (2 * 5)
                {
                    cashDifference += new Cash(cashA.CurrEnum, 0, 0, -1, 1, 2, 0);
                }
                else if (qtyFifty > 0) // 50 => (2 * 20) + (2 * 5)
                {
                    cashDifference += new Cash(cashA.CurrEnum, 0, -1, 2, 0, 2, 0);
                }
                else if (qtyHundred > 0) // 100 => (1 * 50) + (2 * 20) + (2 * 5)
                {
                    cashDifference += new Cash(cashA.CurrEnum, -1, 1, 2, 0, 2, 0);
                }
            }

            cashDifference += new Cash(cashA.CurrEnum, 0, 0, 0, 0, -cashA.QtyFive, 0);

            // Check qtyTen
            if (qtyTen < cashA.QtyTen)
            {
                if (qtyTwenty > 0) // 20 => (2 * 10)
                {
                    cashDifference += new Cash(cashA.CurrEnum, 0, 0, -1, 2, 0, 0);
                }
                else if (qtyFifty > 0) // 50 => (2 * 20) + (1 * 10)
                {
                    cashDifference += new Cash(cashA.CurrEnum, 0, -1, 2, 1, 0, 0);
                }
                else if (qtyHundred > 0) // 100 => (1 * 50) + (2 * 20) + (1* 10)
                {
                    cashDifference += new Cash(cashA.CurrEnum, -1, 1, 2, 1, 0, 0);
                }
            }

            cashDifference += new Cash(cashA.CurrEnum, 0, 0, 0, -cashA.QtyTen, 0, 0);

            // Check qtyTwenty
            if (qtyTwenty < cashA.QtyTwenty)
            {
                if (qtyFifty > 0) // 50 => (2 * 20) + (1 * 10)
                {
                    cashDifference += new Cash(cashA.CurrEnum, 0, -1, 2, 1, 0, 0);
                }
                else if (qtyHundred > 0) // 100 => (1 * 50) + (2 * 20) + (1 * 10)
                {
                    cashDifference += new Cash(cashA.CurrEnum, -1, 1, 2, 1, 0, 0);
                }
            }

            cashDifference += new Cash(cashA.CurrEnum, 0, 0, -cashA.QtyTwenty, 0, 0, 0);

            // Check qtyFifty
            if (qtyFifty < cashA.QtyFifty)
            {
                if (qtyHundred > 0) // 100 => (2 * 50)
                {
                    cashDifference += new Cash(cashA.CurrEnum, -1, 2, 0, 0, 0, 0);
                }
            }

            cashDifference += new Cash(cashA.CurrEnum, 0, -cashA.QtyFifty, 0, 0, 0, 0);

            var newCash =
                new Cash(cashA.CurrEnum, qtyHundred, qtyFifty, qtyTwenty, qtyTen, qtyFive, qtyOne) +
                cashDifference;

            // Check qtyHundred
            if (qtyHundred < cashA.QtyHundred)
            {
                // TODO: Must check backwards to accept subtraction
                // 100 => ? 50 + ? 20 + ? 10 + ? 5 + ? 1
                var neededHundred = cashA.QtyHundred * 100;
                cashA.QtyHundred = 0;

                Console.WriteLine($"neededHundred: {neededHundred}");
                Console.WriteLine($"newCash.QtyFifty: {newCash.QtyFifty}");
                if (newCash.QtyFifty > 0 && neededHundred > 0)
                {
                    Console.Write($"use qtyFifty -> ");
                    if (newCash.QtyFifty * 50 > neededHundred)
                    {
                        var reservedQtyFifty = neededHundred / 50;
                        neededHundred -= reservedQtyFifty * 50;
                        Console.WriteLine(
                            $"new neededHundred {neededHundred} used from reservedQtyFifty:{reservedQtyFifty}");
                        newCash.QtyFifty -= reservedQtyFifty;
                    }
                    else
                    {
                        neededHundred -= newCash.QtyFifty * 50;
                        Console.WriteLine(
                            $"new neededHundred {neededHundred} used from newCash.QtyFifty:{newCash.QtyFifty}");
                        newCash.QtyFifty = 0;
                    }
                }
                else
                {
                    Console.WriteLine($"don't use qtyFifty");
                }

                Console.WriteLine($"newCash.QtyTwenty: {newCash.QtyTwenty}");
                if (newCash.QtyTwenty > 0 && neededHundred > 0)
                {
                    Console.Write($"use qtyTwenty -> ");
                    if (newCash.QtyTwenty * 20 > neededHundred)
                    {
                        var reservedQtyTwenty = neededHundred / 20;
                        neededHundred -= reservedQtyTwenty * 20;
                        Console.WriteLine(
                            $"new neededHundred {neededHundred} used from reservedQtyTwenty:{reservedQtyTwenty}");
                        newCash.QtyTwenty -= reservedQtyTwenty;
                    }
                    else
                    {
                        neededHundred -= newCash.QtyTwenty * 20;
                        Console.WriteLine(
                            $"new neededHundred {neededHundred} used from newCash.QtyTwenty:{newCash.QtyTwenty}");
                        newCash.QtyTwenty = 0;
                    }
                }
                else
                {
                    Console.WriteLine($"don't use qtyTwenty");
                }

                Console.WriteLine($"newCash.QtyTen: {newCash.QtyTen}");
                if (newCash.QtyTen > 0 && neededHundred > 0)
                {
                    Console.Write($"use QtyTen -> ");
                    if (newCash.QtyTen * 10 > neededHundred)
                    {
                        var reservedQtyTen = neededHundred / 10;
                        neededHundred -= reservedQtyTen * 10;
                        Console.WriteLine(
                            $"new neededHundred {neededHundred} used from reservedQtyTen:{reservedQtyTen}");
                        newCash.QtyTen -= reservedQtyTen;
                    }
                    else
                    {
                        neededHundred -= newCash.QtyTen * 10;
                        Console.WriteLine(
                            $"new neededHundred {neededHundred} used from newCash.QtyTen:{newCash.QtyTen}");
                        newCash.QtyTen = 0;
                    }
                }
                else
                {
                    Console.WriteLine($"don't use QtyTen");
                }

                Console.WriteLine($"newCash.QtyTen: {newCash.QtyFive}");
                if (newCash.QtyFive > 0 && neededHundred > 0)
                {
                    Console.Write($"use QtyFive -> ");
                    if (newCash.QtyFive * 5 > neededHundred)
                    {
                        var reservedQtyFive = neededHundred / 5;
                        neededHundred -= reservedQtyFive * 5;
                        Console.WriteLine(
                            $"new neededHundred {neededHundred} used from reservedQtyFive:{reservedQtyFive}");
                        newCash.QtyFive -= reservedQtyFive;
                    }
                    else
                    {
                        neededHundred -= newCash.QtyFive * 5;
                        Console.WriteLine(
                            $"new neededHundred {neededHundred} used from newCash.QtyFive:{newCash.QtyFive}");
                        newCash.QtyFive = 0;
                    }
                }
                else
                {
                    Console.WriteLine($"don't use QtyFive");
                }

                Console.WriteLine($"newCash.QtyOne: {newCash.QtyOne}");
                if (newCash.QtyOne > 0 && neededHundred > 0)
                {
                    Console.Write($"use QtyOne -> ");
                    if (newCash.QtyOne * 1 > neededHundred)
                    {
                        var reservedQtyOne = neededHundred / 1;
                        neededHundred -= reservedQtyOne * 1;
                        Console.WriteLine(
                            $"new neededHundred {neededHundred} used from reservedQtyOne:{reservedQtyOne}");
                        newCash.QtyOne -= reservedQtyOne;
                    }
                    else
                    {
                        neededHundred -= newCash.QtyOne * 5;
                        Console.WriteLine(
                            $"new neededHundred {neededHundred} used from newCash.QtyOne:{newCash.QtyOne}");
                        newCash.QtyOne = 0;
                    }
                }
                else
                {
                    Console.WriteLine($"don't use QtyOne");
                }
            }

            cashDifference += new Cash(cashA.CurrEnum, -cashA.QtyHundred, 0, 0, 0, 0, 0);

            CashDict[cashA.CurrEnum] = newCash + cashDifference;
        }

        private static bool isSubtractAble(Cash cash, Cash cashA)
        {
            return cash.QtyHundred >= cashA.QtyHundred &&
                   cash.QtyFifty >= cashA.QtyFifty &&
                   cash.QtyTwenty >= cashA.QtyTwenty &&
                   cash.QtyTen >= cashA.QtyTen &&
                   cash.QtyFive >= cashA.QtyFive &&
                   cash.QtyOne >= cashA.QtyOne;
        }

        private Cash SolvedCashForOne(Cash myCash, Cash cashA)
        {
            var cashDifference = new Cash(cashA.CurrEnum, 0);
            var qtyOne = CashDict[cashA.CurrEnum].QtyOne;
            var qtyFive = CashDict[cashA.CurrEnum].QtyFive;
            var qtyTen = CashDict[cashA.CurrEnum].QtyTen;
            var qtyTwenty = CashDict[cashA.CurrEnum].QtyTwenty;
            var qtyFifty = CashDict[cashA.CurrEnum].QtyFifty;
            var qtyHundred = CashDict[cashA.CurrEnum].QtyHundred;
            // check qtyOne
            if (qtyOne < cashA.QtyOne)
            {
                var borrowed = false;
                var qtyArray = new[] { qtyFive, qtyTen, qtyTwenty, qtyFifty, qtyHundred };
                var solArray = new[]
                {
                    new Cash(cashA.CurrEnum, 0, 0, 0, 0, -1, 5),
                    new Cash(cashA.CurrEnum, 0, 0, 0, -1, 1, 5),
                    new Cash(cashA.CurrEnum, 0, 0, -1, 1, 1, 5),
                    new Cash(cashA.CurrEnum, 0, -1, 2, 0, 1, 5),
                    new Cash(cashA.CurrEnum, -1, 1, 2, 0, 1, 5)
                };

                var index = 0;
                while (borrowed == false)
                {
                    if (qtyArray[index] > 0)
                    {
                        cashDifference += solArray[index];
                        borrowed = true;
                    }

                    index++;
                }

                Console.WriteLine($"Before: {cashDifference}");
            }

            var solvedCash = myCash + cashDifference + new Cash(cashA.CurrEnum, 0, 0, 0, 0, 0, -cashA.QtyOne);
            return solvedCash;
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

        public class CashesException : Exception
        {
            public enum CashesError
            {
                [Description("This currency doesn't exist")]
                CurrencyDoesntExist,
                
                [Description("The Cash exceeded the available amount")]
                CashExceedAvailableAmount,

                [Description("The Cash Amount could not be subtracted")]
                CashAmountCouldNotBeSubtracted
            }

            public CashesException(CashesError error)
            {
                Message = GetDescription(error);
            }

            private static string GetDescription(CashesError error)
            {
                var field = error.GetType().GetField(error.ToString());
                var attr = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                var description =
                    attr.Length == 0 ? error.ToString() : ((DescriptionAttribute)attr[0]).Description;
                return description;
            }

            public override string Message { get; }
        }
    }
}