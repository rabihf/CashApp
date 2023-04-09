using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using static HelperLibrary.Cash.CurrencyEnum;

namespace HelperLibrary
{
    public class Cash
    {
        public readonly IEnumerable<int> BillsLBP = CreateEnumsList(typeof(CashLBPEnum), true);
        public readonly IEnumerable<int> BillsUSD = CreateEnumsList(typeof(CashUSDEnum), true);

        private CurrencyEnum CurrEnum { get; }
        public const CurrencyEnum DefaultCurrency = LBP;
        private decimal ExchangeRate => (decimal)CurrEnum;

        public const string USDStrFormat = @"{0:N2}";
        public const string LBPStrFormat = @"{0:N0}";


        private decimal Amount
        {
            get
            {
                IEnumerable<int> billsUsed = CurrEnum == LBP ? BillsLBP.ToList() : BillsUSD.ToList();
                decimal amt = QtyHundred * billsUsed.ElementAt(0) + QtyFifty * billsUsed.ElementAt(1) +
                              QtyTwenty * billsUsed.ElementAt(2) + QtyTen * billsUsed.ElementAt(3) +
                              QtyFive * billsUsed.ElementAt(4) + QtyOne * billsUsed.ElementAt(5);

                return amt;
            }
        }

        private int QtyHundred { get; set; }
        private int QtyFifty { get; set; }
        private int QtyTwenty { get; set; }
        private int QtyTen { get; set; }
        private int QtyFive { get; set; }
        private int QtyOne { get; set; }

        private string CurrencyStr => CurrEnum.ToString();
        
        private string StrFormat { get; set; } = "{0, -3}";

        public Cash(CurrencyEnum currencyEnum, int qtyHundred, int qtyFifty, int qtyTwenty, int qtyTen, int qtyFive, int qtyOne)
        {
            QtyHundred = qtyHundred;
            QtyFifty = qtyFifty;
            QtyTwenty = qtyTwenty;
            QtyTen = qtyTen;
            QtyFive = qtyFive;
            QtyOne = qtyOne;
            CurrEnum = currencyEnum;
            // ExchangeRate = (decimal)currencyEnum;
        }

        public Cash()
        {
            CurrEnum = DefaultCurrency;
        }

        public Cash(CurrencyEnum currEnum, decimal amount)
        {
            CurrEnum = currEnum;
            // ExchangeRate = (decimal)currEnum;
            IEnumerable<int> bills = CurrEnum == LBP ? BillsLBP.ToList() : BillsUSD.ToList();
            QtyHundred = (int)(amount / bills.ElementAt(0));
            amount = (int)(amount % bills.ElementAt(0));
            QtyFifty = (int)(amount / bills.ElementAt(1));
            amount = (int)(amount % bills.ElementAt(1));
            QtyTwenty = (int)(amount / bills.ElementAt(2));
            amount = (int)(amount % bills.ElementAt(2));
            QtyTen = (int)(amount / bills.ElementAt(3));
            amount = (int)(amount % bills.ElementAt(3));
            QtyFive = (int)(amount / bills.ElementAt(4));
            amount = (int)(amount % bills.ElementAt(4));
            QtyOne = (int)(amount / bills.ElementAt(5));
        }

        public override string ToString()
        {
            // TODO: must beautify the output
            Console.WriteLine($"Currency: {CurrencyStr}");
            var headers = CurrEnum == LBP ? BillsLBP : BillsUSD;
            var header = headers.Aggregate(string.Empty, (current, str) => current + ($"{str,6}" + " "));
            Console.WriteLine(header);
            Console.WriteLine("-".PadLeft(header.Length,'-'));
            int[] dataArray = { QtyHundred, QtyFifty, QtyTwenty, QtyTen, QtyFive, QtyOne };
            var data = dataArray.Aggregate(string.Empty, (current, str) => current + ($"{str, 6}" + " "));
            Console.WriteLine(data);
            var amount = Amount;
            return CurrencyStr == LBP.ToString()
                ? string.Format(LBPStrFormat, amount) + " " + string.Format(StrFormat, CurrencyStr)
                : string.Format(USDStrFormat, amount) + " " + string.Format(StrFormat, CurrencyStr) + " ~ " +
                  string.Format(LBPStrFormat, amount * ExchangeRate) + " " + string.Format(StrFormat, DefaultCurrency);
        }

        public static Cash operator +(Cash a, decimal b) // overloading
        {
            var cashB = new Cash(a.CurrEnum, b);
            return cashB;
        }

        public static Cash operator +(Cash a, Cash b) // overloading
        {
            Cash cash;
            if (a.CurrEnum == b.CurrEnum)
            {
                Console.WriteLine("Same Currency");
                cash = new Cash(a.CurrEnum,
                    a.QtyHundred + b.QtyHundred,
                    a.QtyFifty + b.QtyFifty,
                    a.QtyTwenty + b.QtyTwenty,
                    a.QtyTen + b.QtyTen,
                    a.QtyFive + b.QtyFive,
                    a.QtyOne + b.QtyOne);
            }
            else
            {
                // TODO: Must figure out how to add different currencies
                Console.WriteLine("Different Currency converted to base currency");
                //cash.Amount = a.AmountToBase * a.ExchangeRate + b.AmountToBase * b.ExchangeRate;
                cash = new Cash(DefaultCurrency,
                    (int)(a.QtyHundred * a.ExchangeRate + b.QtyHundred * b.ExchangeRate),
                    (int)(a.QtyFifty  * a.ExchangeRate+ b.QtyFifty * a.ExchangeRate),
                    (int)(a.QtyTwenty * a.ExchangeRate + b.QtyTwenty * a.ExchangeRate),
                    (int)(a.QtyTen  * a.ExchangeRate+ b.QtyTen * a.ExchangeRate),
                    (int)(a.QtyFive * a.ExchangeRate + b.QtyFive * a.ExchangeRate),
                    (int)(a.QtyOne * a.ExchangeRate + b.QtyOne * a.ExchangeRate));
            }

            return cash;
        }

        public static Cash operator -(Cash a, Cash b) // overloading
        {
            var cash = new Cash();
            if (a.CurrEnum == b.CurrEnum)
            {
                // cash.CurrencyStr = a.CurrencyStr;
                // cash.Amount = a.Amount - b.Amount;
                // cash.ExchangeRate = a.ExchangeRate;
                cash = new Cash(a.CurrEnum,
                    a.QtyHundred - b.QtyHundred,
                    a.QtyFifty - b.QtyFifty,
                    a.QtyTwenty - b.QtyTwenty,
                    a.QtyTen - b.QtyTen,
                    a.QtyFive - b.QtyFive,
                    a.QtyOne - b.QtyOne);
            }

            return cash;
        }

        private enum CashLBPEnum
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

        /// <summary>
        /// Creates a List of ints from an Enum 
        /// </summary>
        /// <param name="enumType"> Enum type</param>
        /// <param name="reversed"> bool default=false ascending order</param>
        /// <returns>List of int from the Enum type in ascending/descending order</returns>
        private static IEnumerable<int> CreateEnumsList(Type enumType, bool reversed = false)
        {
            // Array of enums sorted ascending by int
            const string titleSorted = "Array of enums sorted ascending by int";
            var array = enumType.GetEnumValues();
            DebugPrintEnumType(array, titleSorted, 15, 30);

            if (reversed)
            {
                // Reverse the iEnumerable descending order
                Array.Reverse(array);
                const string titleReverse = "Reverse the Array descending order";
                DebugPrintEnumType(array, titleReverse, 15, 30);
            }

            // Cast the Array to int
            var sortedList = array.Cast<int>().ToList();
            const string titleData = "Cast the Array to sorted List<int>";
            DebugPrintEnumType(sortedList, titleData, 10, 12);

            return sortedList;
        }

        /// <summary>
        /// Helper function to debug information for CreateEnumsList function
        /// </summary>
        /// <param name="iEnumerable">IEnumerable (Array, List, ...)</param>
        /// <param name="title">string: Title</param>
        /// <param name="enumWidth">int: First column width</param>
        /// <param name="typeWidth">int: Second column width</param>
        private static void DebugPrintEnumType(IEnumerable iEnumerable, string title, int enumWidth = 0,
            int typeWidth = 0)
        {
            var tblStrFormat = "{0,-" + enumWidth + "} {1,-" + typeWidth + "}";
            var tblIntFormat = "{0,-" + enumWidth + ":N0} {1,-" + typeWidth + "}";
            var message = string.Format(tblStrFormat, "ENum", "Type");

            Debug.WriteLine(title);
            Debug.WriteLine(message);
            Debug.WriteLine("-".PadLeft(message.Length, '-'));
            foreach (var obj in iEnumerable)
            {
                Debug.WriteLine(obj is int ? tblIntFormat : tblStrFormat, obj, obj.GetType());
            }

            Debug.WriteLine("");
        }
    }
}