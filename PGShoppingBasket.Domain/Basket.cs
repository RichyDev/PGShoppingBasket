using System;
using System.Collections.Generic;

namespace PGShoppingBasket.Domain
{
    public class Basket : Entity, IAggregateRoot
    {
        public Customer Customer { get; }

        private List<BasketProduct> _products = new List<BasketProduct>();
        public IEnumerable<BasketProduct> Products => _products;

        private List<OfferVoucher> _offerVouchers = new List<OfferVoucher>();
        public IEnumerable<OfferVoucher> OfferVouchers => _offerVouchers;

        private List<GiftVoucher> _giftVouchers = new List<GiftVoucher>();
        public IEnumerable<GiftVoucher> GiftVouchers => _giftVouchers;

        private List<string> _messages = new List<string>();
        public IEnumerable<string> Messages => _messages;

        public double Total { get; private set; }

        public Basket(Customer customer)
        {
            Customer = customer;
        }

        public void AddProduct(BasketProduct product)
        {

        }

        public void ApplyOfferVoucher(OfferVoucher offerVoucher)
        {

        }

        public void RedeemGiftVoucher(GiftVoucher giftVoucher)
        {

        }
    }
}
