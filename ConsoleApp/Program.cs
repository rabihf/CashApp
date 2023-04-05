﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HelperLibrary;
using static HelperLibrary.Cash;
using static HelperLibrary.Cash.CurrencyEnum;

namespace ConsoleApp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CountCash();
        }

        private static void CountCash()
        {
            var cashUSD = new Cash(USD, 0, 0, 2, 12, 1, 3);
            var cashLBP = new Cash(LBP, 212, 1, 0, 0, 0, 0);
            Console.WriteLine($"cashUSD: {cashUSD}");
            Console.WriteLine($"cashLBP: {cashLBP}");
            var payUSD = new Cash(USD, 1, 0, 1, 0, 0, 0);

            Console.WriteLine($"payUSD:{payUSD}");
            Console.WriteLine($"cashUSD + payUSD: {cashUSD + payUSD}");
            Console.WriteLine($"cashUSD - payUSD: {cashUSD - payUSD}");
            //
            // var l = typeof(CurrencyEnum).GetEnumValues();
            // foreach (var obj in l)
            // {
            //     Console.WriteLine(obj);
            // }
            //
            // CashEnum test = CashEnum.HundredThousand;
            // Console.WriteLine($"CheckEnum(test): {CheckEnum(test)}");
            // Console.WriteLine($"test: {test}");
            // Console.WriteLine($"(int)test: {(int)test}");

            // var myInt = 100_000;
            // var cashEnum = (CashEnum)myInt;
            // Console.WriteLine($"myInt: {myInt} - cashEnum: {cashEnum}");
            // var foo = (CashEnum)Enum.ToObject(typeof(CashEnum), myInt);
            // var foo2 = (CashEnum)Enum.Parse(typeof(CashEnum), 5_000.ToString());
            // Console.WriteLine($"foo: {foo} - {(int)foo:N0}");
            // Console.WriteLine($"foo2: {foo2} - {(int)foo2:N0}");


            var listLBP = CreateEnumsList(typeof(CashLBPEnum), true);
            var listUSD = CreateEnumsList(typeof(CashUSDEnum), true);

        }

        // private enum CashEnum
        // {
        //     HundredThousand = 100_000,
        //     FiftyThousand = 50_000,
        //     TwentyThousand = 20_000,
        //     TenThousand = 10_000,
        //     FiveThousand = 5_000,
        //     OneThousand = 1_000
        // }
    }
    /*
     C# – How to cast int to enum
     From an int:

        YourEnum foo = (YourEnum)yourInt;

    From a string:

        YourEnum foo = (YourEnum) Enum.Parse(typeof(YourEnum), yourString);

        // The foo.ToString().Contains(",") check is necessary for enumerations marked with an [Flags] attribute
        if (!Enum.IsDefined(typeof(YourEnum), foo) && !foo.ToString().Contains(","))
        {
            throw new InvalidOperationException($"{yourString} is not an underlying value of the YourEnum enumeration.")
        }

    Update:

    From number you can also

        YourEnum foo = (YourEnum)Enum.ToObject(typeof(YourEnum) , yourInt);
     
     */
}