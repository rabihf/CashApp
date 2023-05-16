using System;
using HelperLibrary;
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
            Console.WriteLine($"cashUSD: {cashUSD}\n");
            cashes.Add(cashUSD);
            var cashA = new Cash(USD, 0,0,1,13,1,3);
            Console.WriteLine($"cashA: {cashA}");

            TestSubtract(cashes, cashA);


            


            Console.WriteLine();
        }

        private static void TestSubtract(Cashes cashes, Cash cashA)
        {
            try
            {
                cashes.Subtract(cashA);
            }
            catch (Cashes.CashesException ce)
            {
                Console.WriteLine(ce.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                Console.WriteLine(cashes);
            }
        }
    }
}