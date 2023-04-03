using System;
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
            var cashUSD = new Cash(USD,0, 0, 2, 12, 1, 3);
            var cashLBP = new Cash(LBP, 212, 1, 0, 0, 0, 0);
            Console.WriteLine($"cashUSD: {cashUSD}");
            Console.WriteLine($"cashLBP: {cashLBP}");
            var payUSD = new Cash(USD, 1, 0, 1, 0, 0, 0);

            Console.WriteLine($"payUSD:{payUSD}");
            Console.WriteLine($"cashUSD + payUSD: {cashUSD + payUSD}");
            Console.WriteLine($"cashUSD - payUSD: {cashUSD - payUSD}");

            var l = typeof(CurrencyEnum).GetEnumValues();
            foreach (var obj in l)
            {
                Console.WriteLine(obj);
            }

        }
    }
}