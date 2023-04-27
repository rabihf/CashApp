using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HelperLibrary;
using static HelperLibrary.Cash;
using static HelperLibrary.CurrencyEnum;

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
            // Console.WriteLine($"cashUSD: {cashUSD}\n");
            // Console.WriteLine($"cashLBP: {cashLBP}\n");
            var cashes = new Cashes();
            Cashes.Add(cashLBP);
            Cashes.Add(new Cash(LBP, 1750000));
            Cashes.Add(cashUSD);
            Cashes.Add(new Cash(USD, 32));

            // Cashes.Add(cashUSD);
            
            // foreach (var currency in cashes.Currencies)
            // {
            //     Console.WriteLine($"{currency}: {currency.GetType()}");
            // }

            Console.WriteLine(cashes);

            

            Console.WriteLine();
        }
    }
}