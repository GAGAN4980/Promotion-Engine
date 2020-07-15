using System;
using System.Collections.Generic;

namespace Promotion_Engine
{
    public class PromotionEngine
    {
        static void Main(string[] args)
        {
            PromotionCalculater promotionCalculater = new PromotionCalculater();
            Dictionary<string, int> salesInvoice = new Dictionary<string, int>() { { "A", 1 }, { "B", 1 }, { "C", 1 }, { "D", 1 }};
            float totalPrice = promotionCalculater.CalculatePromotion(salesInvoice);
            if(totalPrice == 110.0)
            {
                Console.WriteLine("TotalPrice = "+ 110.0);
            }
        }
    }      
}
