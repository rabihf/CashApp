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
            var cashes = new Cashes();
            var cashUSD = new Cash(USD, 0, 0, 2, 12, 1, 3);
            //var cashLBP = new Cash(LBP, 212, 1, 0, 0, 0, 0);
            // Console.WriteLine($"cashUSD: {cashUSD}\n");
            // Console.WriteLine($"cashLBP: {cashLBP}\n");
            //cashes.Add(cashLBP);
            //cashes.Add(cashLBP);
            // cashes.Add(new Cash(LBP, 1_850_000));
            cashes.Add(cashUSD);
            // cashes.Add(new Cash(USD, 32));
            // cashes.Add(new Cash(LBP, 1, 0, 0, 0, 0, 0));
            //var cashes = cashUSD + cashLBP;
            //cashes.Add(cashLBP + cashUSD);
            // var sum = cashLBP + cashUSD;
            // cashes.Add(sum);
            Console.WriteLine(cashes);
            cashes.Subtract(new Cash(USD, 150));
            Console.WriteLine(cashes);
            

            
            
            
            
            
            
            
            // Console.WriteLine(cashes);

            

            Console.WriteLine();
        }
    }
}