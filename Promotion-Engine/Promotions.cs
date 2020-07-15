using System.Collections.Generic;
using System.Linq;

namespace Promotion_Engine
{
    // The Configured promotions rules.
    public class Promotions
    {
        private readonly List<PromotionOffer> promotionsList;
        public Promotions()
        {
            promotionsList = new List<PromotionOffer>();
            PromotionOffer A = new PromotionOffer();
            A.ProductOffer.Add("A", 3);
            A.OfferPrice = 130;
            promotionsList.Add(A);

            PromotionOffer B = new PromotionOffer();
            B.ProductOffer.Add("B", 2);
            B.OfferPrice = 45;
            promotionsList.Add(B);

            PromotionOffer CandD = new PromotionOffer();
            CandD.ProductOffer.Add("C", 1);
            CandD.ProductOffer.Add("D", 1);
            CandD.OfferPrice = 30;
            promotionsList.Add(CandD);
        }
        public List<PromotionOffer> GetPromotions()
        {
            return promotionsList;
        }
    }
    public class PromotionOffer
    {
        public Dictionary<string, int> ProductOffer = new Dictionary<string, int>();
        public float OfferPrice { get; set; }       
    }
    public class PromotionCalculater
    {
        private readonly Dictionary<string,float> products;

        public PromotionCalculater() {
            // The SKUId's list with unit price.
            products = new Dictionary<string, float>();
            products.Add("A", 50);
            products.Add("B", 30);
            products.Add("C", 20);
            products.Add("D", 15);
        }
 
        // Calculates the  total price of the sales invoice after applying the promotions.
        public float CalculatePromotion(Dictionary<string, int> SalesInvoice)
        {
            Promotions promotions = new Promotions();
            float totalPrice = 0;
            float promotionsOffers = 0;
            // Apply each promotion on the sales invoice and get the aggreageted offervalue.     
            foreach (var promotion in promotions.GetPromotions())
            {
                var offer = ApplyPromotion(promotion, SalesInvoice);
                promotionsOffers += offer;
            }

            // Calculate the total price for non promotion items, ie left over items after aplying promotion
            var saleList = new List<string>(SalesInvoice.Keys);
            foreach (var sale in saleList)
            {
                var price = products[sale] * SalesInvoice[sale];
                totalPrice += price;
            }
            return promotionsOffers + totalPrice;
        }

        // Apply the promotions on the the sales invoice.
        public float ApplyPromotion(PromotionOffer promotions, Dictionary<string, int> SalesInvoice)
        {
            // Single promotion
            if (promotions.ProductOffer.Count == 1)
            {
                var key = promotions.ProductOffer.ElementAt(0).Key;
                if (SalesInvoice.ContainsKey(key))
                {
                    int quantityInvoice = SalesInvoice[key];
                    int offerQuantity = promotions.ProductOffer[key];
                    int offerscount = quantityInvoice / offerQuantity;
                    int remainingQuantity = quantityInvoice % offerQuantity;
                    SalesInvoice[key] = remainingQuantity;
                    return offerscount * promotions.OfferPrice;
                }
            }
            else
            {   // Combo offer for 2 products
                var key1 = promotions.ProductOffer.ElementAt(0).Key;
                var key2 = promotions.ProductOffer.ElementAt(1).Key; ;
                if (SalesInvoice.ContainsKey(key1) && SalesInvoice.ContainsKey(key2))
                {
                    int quantityInvoice1 = SalesInvoice[key1];
                    int offerQuantity1 = promotions.ProductOffer[key1];
                    int offerscount1 = quantityInvoice1 / offerQuantity1;
                    int remainingQuantity1 = quantityInvoice1 % offerQuantity1;

                    int quantityInvoice2 = SalesInvoice[key2];
                    int offerQuantity2 = promotions.ProductOffer[key2];
                    int offerscount2 = quantityInvoice2 / offerQuantity2;
                    int remainingQuantity2 = quantityInvoice2 % offerQuantity2;

                    if (offerscount1 <= offerscount2) // combo offer with least offer counts.
                    {
                        SalesInvoice[key1] = remainingQuantity1;
                        SalesInvoice[key2] = remainingQuantity2 + (offerQuantity2 * (offerscount2 - offerscount1));
                        return offerscount1 * promotions.OfferPrice;
                    }
                    else
                    {
                        SalesInvoice[key1] = remainingQuantity1 + (offerQuantity1 * (offerscount1 - offerscount2));
                        SalesInvoice[key2] = remainingQuantity2;
                        return offerscount2 * promotions.OfferPrice;
                    }
                }
            }
            return 0;
        }
    }
}
