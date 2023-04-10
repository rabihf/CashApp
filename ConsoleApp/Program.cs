using System;
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
            Console.WriteLine($"cashUSD: {cashUSD}\n");
            // Console.WriteLine($"cashLBP: {cashLBP}\n");

            Console.WriteLine($"{cashUSD + 132}");
            
            
            // var payUSD = new Cash(USD, 1, 1, 1, 1, 1, 1);
            
            // Console.WriteLine($"payUSD: {payUSD}\n");
            
            // Console.WriteLine($"SUM: {cashLBP + payUSD}");
            // Console.WriteLine($"SUM: {cashUSD + payUSD}");
            // Console.WriteLine($"cashLBP + payUSD: {cashLBP + payUSD}");
            // Console.WriteLine($"cashLBP - payUSD: {cashLBP - payUSD}");
            // Console.WriteLine($"cashUSD + payUSD: {cashUSD + payUSD}");

            // var cash114 = new Cash(USD, 132);
            // Console.WriteLine($"114USD: {cash114}");
            // var cash8750000 = new Cash(LBP, 8750000);
            // Console.WriteLine($"cash12540000: {cash12540000}");
            //Console.WriteLine($"cashUSD + 114USD: {cashUSD + cash114}");
            // Console.WriteLine(cashLBP);
            // Console.WriteLine($"cashLBP + cash12540000: {cashLBP + cash8750000}");
            // Console.WriteLine($"cashUSD - payUSD: {cashUSD - payUSD}");
            // Console.WriteLine($"{cashLBP + 8750000M}");
            

            Console.WriteLine();
        }
    }
}