using System;
using System.Collections.Generic;
using System.Linq;

namespace PGShoppingBasket.Domain
{
    public class Basket : Entity, IAggregateRoot
    {
        public Customer Customer { get; }

        private readonly List<BasketProduct> _products = new List<BasketProduct>();
        public IEnumerable<BasketProduct> Products => _products;

        private readonly List<OfferVoucher> _offerVouchers = new List<OfferVoucher>();
        public IEnumerable<OfferVoucher> OfferVouchers => _offerVouchers;

        private readonly List<GiftVoucher> _giftVouchers = new List<GiftVoucher>();
        public IEnumerable<GiftVoucher> GiftVouchers => _giftVouchers;

        private readonly List<string> _messages = new List<string>();
        public IEnumerable<string> Messages => _messages;

        public decimal Total { get; private set; }

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

            UpdateBasket();
        }

        public void ApplyOfferVoucher(OfferVoucher offerVoucher)
        {
            if (_offerVouchers.Any(x => x.Code == offerVoucher.Code))
                throw new Exception($"Basket already has Offer Voucher with code '{offerVoucher.Code}' applied");

            _offerVouchers.Add(offerVoucher);

            UpdateBasket();
        }

        public void RedeemGiftVoucher(GiftVoucher giftVoucher)
        {
            if (_giftVouchers.Any(x => x.Code == giftVoucher.Code))
                throw new Exception($"Basket already has Gift Voucher with code '{giftVoucher.Code}' applied");

            _giftVouchers.Add(giftVoucher);

            UpdateBasket();
        }

        private void UpdateBasket()
        {
            var productTotal = 0.00m;

            foreach (var p in _products)
            {
                productTotal += p.Total;
            }

            var total = productTotal;

            foreach (var o in _offerVouchers)
            {
                if(_products.All(x => x.Category != o.Category))
                    _messages.Add($"There are no products in your basket applicable to voucher Voucher {o.Code}");
                else if (o.BasketThreshold > productTotal)
                    total -= o.Amount;
            }

            foreach (var g in _giftVouchers)
            {
                total -= g.Amount;
            }

            Total = total;
        }
    }
}
