using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using static HelperLibrary.CurrencyEnum;

namespace HelperLibrary
{
    public class Cash
    {
        public readonly IEnumerable<int> BillsLBP = CreateEnumsList(typeof(CashLBPEnum), true);
        public readonly IEnumerable<int> BillsUSD = CreateEnumsList(typeof(CashUSDEnum), true);

        internal CurrencyEnum CurrEnum { get; }
        public const CurrencyEnum DefaultCurrency = LBP;
        private decimal ExchangeRate => (decimal)CurrEnum;

        public const string USDStrFormat = @"{0:N2}";
        public const string LBPStrFormat = @"{0:N0}";


        public decimal Amount
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

        public int QtyHundred { get; private set; }
        public int QtyFifty { get; private set; }
        public int QtyTwenty { get; private set; }
        public int QtyTen { get; private set; }
        public int QtyFive { get; private set; }
        public int QtyOne { get; private set; }

        private string CurrencyStr => CurrEnum.ToString();

        private string StrFormat { get; set; } = "{0, -3}";

        public Cash(CurrencyEnum currencyEnum, int qtyHundred, int qtyFifty, int qtyTwenty, int qtyTen, int qtyFive,
            int qtyOne)
        {
            QtyHundred = qtyHundred;
            QtyFifty = qtyFifty;
            QtyTwenty = qtyTwenty;
            QtyTen = qtyTen;
            QtyFive = qtyFive;
            QtyOne = qtyOne;
            CurrEnum = currencyEnum;
            // CashesDict = new Cashes();
        }

        public Cash()
        {
            CurrEnum = DefaultCurrency;
            // CashesDict = new Cashes();
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
            // CashesDict = new Cashes();
        }

        // private static Cashes CashesDict { get; set; }

        public override string ToString()
        {
            // TODO: must beautify the output
            string output = string.Empty;
            output += $"\nCurrency: {CurrencyStr} - ";
            var amount = Amount;
            output += CurrencyStr == LBP.ToString()
                ? string.Format(LBPStrFormat, amount) + " " + string.Format(StrFormat, CurrencyStr) + "\n"
                : string.Format(USDStrFormat, amount) + " " + string.Format(StrFormat, CurrencyStr) + " ~ " +
                  string.Format(LBPStrFormat, amount * ExchangeRate) + " " + string.Format(StrFormat, DefaultCurrency) +
                  "\n";
            var headers = CurrEnum == LBP ? BillsLBP : BillsUSD;
            var header = headers.Aggregate(string.Empty, (current, str) => current + ($"{str,6}" + " "));
            output += header + "\n";
            output += "-".PadLeft(header.Length, '-') + "\n";
            int[] dataArray = { QtyHundred, QtyFifty, QtyTwenty, QtyTen, QtyFive, QtyOne };
            var data = dataArray.Aggregate(string.Empty, (current, str) => current + ($"{str,6}" + " "));
            output += data + "\n";

            return output;
        }

        public static Cashes operator +(Cash a, decimal b) // overloading
        {
            return a + new Cash(a.CurrEnum, b);
        }

        public static Cashes operator +(Cash a, Cash b) // overloading
        {
            var cashes = new Cashes();
            if (a.CurrEnum == b.CurrEnum)
            {
                Console.WriteLine("Same Currency Adding bills");
                var cash = new Cash(a.CurrEnum,
                    a.QtyHundred + b.QtyHundred,
                    a.QtyFifty + b.QtyFifty,
                    a.QtyTwenty + b.QtyTwenty,
                    a.QtyTen + b.QtyTen,
                    a.QtyFive + b.QtyFive,
                    a.QtyOne + b.QtyOne);
                cashes.Add(cash);
            }
            else
            {
                Console.WriteLine("Different Currency");
                cashes.Add(a);
                cashes.Add(b);
            }
            return cashes;
        }

        // public void Add(Cash a, Cash b)
        // {
        //     var cash = new Cash(a.CurrEnum,
        //         a.QtyHundred + b.QtyHundred,
        //         a.QtyFifty + b.QtyFifty,
        //         a.QtyTwenty + b.QtyTwenty,
        //         a.QtyTen + b.QtyTen,
        //         a.QtyFive + b.QtyFive,
        //         a.QtyOne + b.QtyOne);
        // }

        public static Cashes operator -(Cash a, decimal b) // overloading
        {
            return a - new Cash(a.CurrEnum, b);
        }

        public static Cashes operator -(Cash a, Cash b) // overloading
        {
            var cashes = new Cashes();
            if (a.CurrEnum == b.CurrEnum)
            {
                Console.WriteLine("Same Currency Subtracting bills");
                var cash = a.Amount >= b.Amount
                    ? new Cash(a.CurrEnum, a.Amount - b.Amount)
                    : new Cash(a.CurrEnum, b.Amount - a.Amount);
                cashes.Add(cash);
            }
            else
            {
                Console.WriteLine("Different Currency converting to base currency");
                var cashAConverted = new Cash(LBP, a.Amount * a.ExchangeRate);
                var cashBConverted = new Cash(LBP, b.Amount * b.ExchangeRate);
                var newCash = cashAConverted.Amount >= cashBConverted.Amount
                    ? new Cash(LBP, cashAConverted.Amount - cashBConverted.Amount)
                    : new Cash(LBP, cashBConverted.Amount - cashAConverted.Amount);
                cashes.Add(newCash);
            }
            return cashes;
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