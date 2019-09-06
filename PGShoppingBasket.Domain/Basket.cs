using System;
using System.Collections.Generic;
using System.Linq;

namespace PGShoppingBasket.Domain
{
    public class Basket : Entity, IAggregateRoot
    {
        private bool _isDirty = false;

        public Customer Customer { get; }

        private readonly List<BasketProduct> _products = new List<BasketProduct>();
        public IEnumerable<BasketProduct> Products => _products;

        private readonly List<OfferVoucher> _offerVouchers = new List<OfferVoucher>();
        public IEnumerable<OfferVoucher> OfferVouchers => _offerVouchers;

        private readonly List<GiftVoucher> _giftVouchers = new List<GiftVoucher>();
        public IEnumerable<GiftVoucher> GiftVouchers => _giftVouchers;

        private readonly List<string> _messages = new List<string>();
        public IEnumerable<string> Messages => _messages;

        public Basket(Customer customer)
        {
            Customer = customer;
        }

        public void AddProduct(BasketProduct product)
        {
            var existingProduct = _products.SingleOrDefault(x => x.Id == product.Id);

            if (existingProduct != null)
            {
                // Allows the same product to be added multiple times, with the quantity updated
                // This also allows the price to be changed if it was updated whilst the basket existed
                var originalQuantity = existingProduct.Quantity;
                product.Quantity += originalQuantity;
                _products.Remove(existingProduct);
                _products.Add(product);
            }
            else
            {
                _products.Add(product);
            }

            _isDirty = true;
        }

        public void ApplyOfferVoucher(OfferVoucher offerVoucher)
        {
            if (_offerVouchers.Any(x => x.Code == offerVoucher.Code))
                throw new Exception($"Basket already has Offer Voucher with code '{offerVoucher.Code}' applied");

            _offerVouchers.Add(offerVoucher);

            _isDirty = true;
        }

        public void RedeemGiftVoucher(GiftVoucher giftVoucher)
        {
            if (_giftVouchers.Any(x => x.Code == giftVoucher.Code))
                throw new Exception($"Basket already has Gift Voucher with code '{giftVoucher.Code}' applied");

            _giftVouchers.Add(giftVoucher);

            _isDirty = true;
        }

        public decimal GetTotal()
        {
            var productTotal = 0.00m;

            foreach (var p in _products)
            {
                productTotal += p.Total;
            }

            var total = productTotal;

            foreach (var o in _offerVouchers)
            {
                var discount = o.Amount;

                if (o.Category != null)
                {
                    // This is a voucher for a specific product category. So get those products
                    var categoryProducts = _products.Where(x => x.Category == o.Category).ToList();

                    if (categoryProducts.Count == 0)
                    {
                        _messages.Add($"There are no products in your basket applicable to voucher Voucher {o.Code}");
                        break;
                    }

                    // See what the total of the qualifying category vouchers is
                    var categoryProductsTotal = categoryProducts.Sum(x => x.Total);

                    // if the qualifying total is less than the voucher then use the qualifying total
                    discount = o.Amount >= categoryProductsTotal ? categoryProductsTotal : o.Amount;
                }

                // Make sure the total of the products is enough to reach the voucher threshold
                if (productTotal > o.BasketThreshold)
                {
                    total -= discount;
                }
            }

            foreach (var g in _giftVouchers)
            {
                total -= g.Amount;
            }

            return total;
        }
    }
}
