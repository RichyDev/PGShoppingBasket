using System;
using System.Collections.Generic;

namespace PGShoppingBasket.Domain
{
    public class Basket : Entity, IAggregateRoot
    {
        public Customer Customer { get; }

        private readonly Dictionary<Guid, BasketProduct> _products = new Dictionary<Guid, BasketProduct>();
        public IEnumerable<BasketProduct> Products => _products.Values;

        private readonly Dictionary<string, OfferVoucher> _offerVouchers = new Dictionary<string, OfferVoucher>();
        public IEnumerable<OfferVoucher> OfferVouchers => _offerVouchers.Values;

        private readonly Dictionary<string, GiftVoucher> _giftVouchers = new Dictionary<string, GiftVoucher>();
        public IEnumerable<GiftVoucher> GiftVouchers => _giftVouchers.Values;

        private readonly List<string> _messages = new List<string>();
        public IEnumerable<string> Messages => _messages;

        public decimal Total { get; private set; }

        public Basket(Customer customer)
        {
            Customer = customer;
        }

        public void AddProduct(BasketProduct product)
        {
            if (_products.ContainsKey(product.Id))
            {
                // Allows the same product to be added multiple times, with the quantity updated
                var originalQuantity = _products[product.Id].Quantity;
                product.Quantity += originalQuantity;
                _products[product.Id] = product;
            }
            else
            {
                _products.Add(product.Id, product);
            }

            UpdateBasket();
        }

        public void ApplyOfferVoucher(OfferVoucher offerVoucher)
        {
            if (_offerVouchers.ContainsKey(offerVoucher.Code))
                throw new Exception($"Basket already has Offer Voucher with code '{offerVoucher.Code}' applied");

            _offerVouchers.Add(offerVoucher.Code, offerVoucher);

            UpdateBasket();
        }

        public void RedeemGiftVoucher(GiftVoucher giftVoucher)
        {
            if (_giftVouchers.ContainsKey(giftVoucher.Code))
                throw new Exception($"Basket already has Gift Voucher with code '{giftVoucher.Code}' applied");

            _giftVouchers.Add(giftVoucher.Code, giftVoucher);

            UpdateBasket();
        }

        private void UpdateBasket()
        {
            var productTotal = 0.00m;

            foreach (var p in _products)
            {
                productTotal += p.Value.Total;
            }

            var total = productTotal;

            foreach (var o in _offerVouchers)
            {
                if (o.Value.BasketThreshold > productTotal)
                    total -= o.Value.Amount;
            }

            foreach (var g in _giftVouchers)
            {
                total -= g.Value.Amount;
            }

            Total = total;
        }
    }
}
