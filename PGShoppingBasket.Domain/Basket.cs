using System;
using System.Collections.Generic;
using System.Linq;

namespace PGShoppingBasket.Domain
{
    public class Basket : Entity, IAggregateRoot
    {
        public bool IsDirty { get; private set; }

        private Customer _customer;

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
            _customer = customer;
        }

        public void AddProduct(Product product, int quantity = 1)
        {
            var existingProduct = _products.SingleOrDefault(x => x.Product.Id == product.Id);

            if (existingProduct != null)
            {
                // Allows the same product to be added multiple times, with the quantity updated
                // This also allows the price, name, etc.. to be changed if it was updated whilst the basket existed
                existingProduct.Quantity += quantity;
                existingProduct.Product = product;
            }
            else
            {
                _products.Add(new BasketProduct(product, quantity));
            }

            IsDirty = true;
        }

        public void ApplyOfferVoucher(OfferVoucher offerVoucher)
        {
            if (_offerVouchers.Any(x => x.Code == offerVoucher.Code))
                throw new Exception($"Basket already has Offer Voucher with code '{offerVoucher.Code}' applied");

            _offerVouchers.Add(offerVoucher);

            IsDirty = true;
        }

        public void RedeemGiftVoucher(GiftVoucher giftVoucher)
        {
            if (_giftVouchers.Any(x => x.Code == giftVoucher.Code))
                throw new Exception($"Basket already has Gift Voucher with code '{giftVoucher.Code}' applied");

            _giftVouchers.Add(giftVoucher);

            IsDirty = true;
        }

        public decimal GetTotal()
        {
            var total = _products.Sum(x => x.Product.Price);

            total = ApplyOfferVouchers(total);

            total = ApplyGiftVouchers(total);

            return total;
        }

        private decimal ApplyOfferVouchers(decimal total)
        {
            var discountableProductsTotal = _products.Where(x => x.Product.Category == null || 
                                                                 !x.Product.Category.CannotBeDiscounted).Sum(x => x.Product.Price);

            foreach (var voucher in _offerVouchers)
            {
                var discount = GetVoucherOfferDiscount(voucher);

                // Make sure the total of the products is enough to reach the voucher threshold
                if (voucher.BasketThreshold.HasValue && discountableProductsTotal <= voucher.BasketThreshold.Value)
                {
                    var amountNeeded = voucher.BasketThreshold.Value - discountableProductsTotal + 0.01m; //The 1p is needed to take it over the threshold
                    _messages.Add($"You have not reached the spend threshold for voucher {voucher.Code}. Spend another £{amountNeeded} to receive £{voucher.Amount} discount from your basket total.");
                    continue;
                }

                total -= discount;
            }

            return total;
        }

        private decimal GetVoucherOfferDiscount(OfferVoucher voucher)
        {
            var discount = voucher.Amount;

            if (voucher.Category != null)
            {
                // This is a voucher for a specific product category. So get those products
                var categoryProducts = _products.Where(x => x.Product.Category == voucher.Category).ToList();

                if (categoryProducts.Count == 0)
                {
                    _messages.Add($"There are no products in your basket applicable to voucher Voucher {voucher.Code}");
                    return 0.00m;
                }

                // See what the total of the qualifying category vouchers is
                var categoryProductsTotal = categoryProducts.Sum(x => x.Total);

                // if the qualifying total is less than the voucher then use the qualifying total
                discount = voucher.Amount >= categoryProductsTotal ? categoryProductsTotal : voucher.Amount;
            }

            return discount;
        }

        private decimal ApplyGiftVouchers(decimal total)
        {
            foreach (var g in _giftVouchers)
            {
                // TODO What if total goes below 0?
                total -= g.Amount;
            }

            return total;
        }
    }
}
