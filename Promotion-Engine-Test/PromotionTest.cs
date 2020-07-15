using NUnit.Framework;
using Promotion_Engine;
using System.Collections.Generic;

namespace Promotion_Engine_Test
{
        public class PromotionTest
        {
            readonly PromotionCalculater promotionCalculater = new PromotionCalculater();

            [TestCase("A", 2, 100)]
            [TestCase("A", 3, 130)]
            [TestCase("B", 2, 45)]
            [TestCase("B", 3, 75)]
            [TestCase("C", 3, 60)]
            [TestCase("D", 4, 60)]
            public void Single_product_promotion(string product, int quantity, float expectedPrice)
            {
                Dictionary<string, int> salesInvoice = new Dictionary<string, int>();
                salesInvoice.Add(product, quantity);
                float totalPrice = promotionCalculater.CalculatePromotion(salesInvoice);
                Assert.AreEqual(totalPrice, expectedPrice);
            }

            [TestCase("A", 3, 150)]
            [TestCase("B", 3, 90)]
            public void Single_product_promotion_Negative(string product, int quantity, float expectedPrice)
            {
                Dictionary<string, int> salesInvoice = new Dictionary<string, int>();
                salesInvoice.Add(product, quantity);
                float totalPrice = promotionCalculater.CalculatePromotion(salesInvoice);
                Assert.AreNotEqual(totalPrice, expectedPrice);
            }

            [Test, TestCaseSource("MultiProducts")]
            public void Multiple_product_promotion(Dictionary<string, int> salesInvoice, float expectedPrice)
            {
                float totalPrice = promotionCalculater.CalculatePromotion(salesInvoice);
                Assert.AreEqual(totalPrice, expectedPrice);
            }
            [Test, TestCaseSource("MultiProductsNegative")]
            public void Multiple_product_promotion_Negative(Dictionary<string, int> salesInvoice, float expectedPrice)
            {
                float totalPrice = promotionCalculater.CalculatePromotion(salesInvoice);
                Assert.AreNotEqual(totalPrice, expectedPrice);
            }
            public static object[] MultiProducts =
            {

                 new object[]{new Dictionary<string, int>(){{"A",1},{"B",1},{"C",1},{"D",1}},110 },
                 new object[]{new Dictionary<string, int>(){{"A",1},{"B",1},{"C",3},{"D",2}},160 },
                 new object[]{new Dictionary<string, int>(){{"A",5},{"B",5},{"C",1}},370 },
                 new object[]{new Dictionary<string, int>(){{"A",3},{"B",5},{"C",1},{"D",1}},280 },
             };

            public static object[] MultiProductsNegative =
            {
                 new object[]{new Dictionary<string, int>(){{"A",1},{"B",1},{"C",1},{"D",1}},105 },
                 new object[]{new Dictionary<string, int>(){{"A",1},{"B",2},{"C",1},{"D",1}},135 },
                 new object[]{new Dictionary<string, int>(){{"A",3},{"B",1},{"C",1},{"D",1}},215 },
                 new object[]{new Dictionary<string, int>(){{"A",3},{"B",2}},210 },
             };

        }
}