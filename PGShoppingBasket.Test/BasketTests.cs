using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using NUnit.Framework;
using PGShoppingBasket.Domain;

namespace PGShoppingBasket.Test
{
    [TestFixture]
    public class BasketTests
    {
        private readonly Customer _customer;
        private readonly ProductCategory _headWearCategory;
        private readonly ProductCategory _topsCategory;
        private readonly ProductCategory _headGearCategory;
        private readonly ProductCategory _giftVoucherCategory;

        private readonly BasketProduct _expensiveHatProduct;
        private readonly BasketProduct _cheapHatProduct;
        private readonly BasketProduct _expensiveJumperProduct;
        private readonly BasketProduct _cheapJumperProduct;
        private readonly BasketProduct _headLightProduct;
        private readonly BasketProduct _thirtyPoundGiftVoucherProduct;

        private readonly GiftVoucher _fivePoundGiftVoucher;

        private readonly OfferVoucher _headGearOfferVoucher;
        private readonly OfferVoucher _fiveOffFiftyOfferVoucher;

        public BasketTests()
        {
            _customer = new Customer();
            _headWearCategory = new ProductCategory("Head Wear");
            _topsCategory = new ProductCategory("Tops");
            _headGearCategory = new ProductCategory("Head Gear");
            _giftVoucherCategory = new ProductCategory("Gift Vouchers");

            _expensiveHatProduct = new BasketProduct("Hat", 25.00m, _headWearCategory, 1);
            _cheapHatProduct = new BasketProduct("Hat", 10.50m, _headWearCategory, 1);
            _expensiveJumperProduct = new BasketProduct("Jumper", 54.65m, _topsCategory, 1);
            _cheapJumperProduct = new BasketProduct("Jumper", 26.00m, _topsCategory, 1);
            _headLightProduct = new BasketProduct("Hat", 10.50m, _headGearCategory, 1);
            _thirtyPoundGiftVoucherProduct = new BasketProduct("£30 Gift Voucher ", 30.00m, _giftVoucherCategory, 1);

            _fivePoundGiftVoucher = new GiftVoucher("XXX-XXX", 5.00m);

            _headGearOfferVoucher = new OfferVoucher("YYY-YYY", 5.00m, 50.00m, _headGearCategory);
            _fiveOffFiftyOfferVoucher = new OfferVoucher("YYY-YYY", 5.00m, 50.00m);
        }

        // Scenario: Basket 1
        [Test]
        public void GivenItemsInBasket_WhenApplyGiftVoucher_ThenTotalIsCorrect()
        {
            // Arrange
            var basket = new Basket(_customer);

            // Act
            basket.AddProduct(_cheapHatProduct);
            basket.AddProduct(_expensiveJumperProduct);
            basket.RedeemGiftVoucher(_fivePoundGiftVoucher);

            TestContext.Write(basket.Overview());

            // Assert
            Assert.AreEqual(60.15, basket.Total);
            Assert.IsEmpty(basket.Messages);
        }

        // Scenario: Basket 2
        [Test]
        public void GivenItemsInBasket_WhenApplyOfferVoucherNotReachingThreshold_ThenTotalIsCorrectAndMessageDisplayed()
        {
            // Arrange
            var basket = new Basket(_customer);

            // Act
            basket.AddProduct(_expensiveHatProduct);
            basket.AddProduct(_cheapJumperProduct);
            basket.ApplyOfferVoucher(_headGearOfferVoucher);

            TestContext.Write(basket.Overview());

            // Assert
            Assert.AreEqual(51.00, basket.Total);
            Assert.Contains("There are no products in your basket applicable to voucher Voucher YYY-YYY", basket.Messages.ToArray());
        }

        // Scenario: Basket 3
        [Test]
        public void GivenItemsInBasket_WhenApplyOfferVoucherReachingThreshold_ThenDiscountDoesNotExceedQualifyingItemPriceAndTotalIsCorrect()
        {
            // Arrange
            var basket = new Basket(_customer);

            // Act
            basket.AddProduct(_expensiveHatProduct);
            basket.AddProduct(_cheapJumperProduct);
            basket.AddProduct(_headLightProduct);
            basket.ApplyOfferVoucher(_headGearOfferVoucher);

            TestContext.Write(basket.Overview());

            // Assert
            Assert.AreEqual(51.00, basket.Total);
            Assert.IsEmpty(basket.Messages);
        }

        // Scenario: Basket 4
        [Test]
        public void GivenItemsInBasket_WhenApplyGiftVoucherAndApplyOfferVoucher_ThenTotalIsCorrect()
        {
            // Arrange
            var basket = new Basket(_customer);

            // Act
            basket.AddProduct(_expensiveHatProduct);
            basket.AddProduct(_cheapJumperProduct);
            basket.RedeemGiftVoucher(_fivePoundGiftVoucher);
            basket.ApplyOfferVoucher(_fiveOffFiftyOfferVoucher);

            TestContext.Write(basket.Overview());

            // Assert
            Assert.AreEqual(41.00, basket.Total);
            Assert.IsEmpty(basket.Messages);
        }

        // Scenario: Basket 5
        [Test]
        public void GivenItemsInBasketIncludingGiftVoucher_WhenApplyOfferVoucherNotReachingThreshold_ThenTotalIsCorrectAndMessageDisplayed()
        {
            // Arrange
            var basket = new Basket(_customer);

            // Act
            basket.AddProduct(_expensiveHatProduct);
            basket.AddProduct(_thirtyPoundGiftVoucherProduct);
            basket.ApplyOfferVoucher(_fiveOffFiftyOfferVoucher);

            TestContext.Write(basket.Overview());

            // Assert
            Assert.AreEqual(55.00, basket.Total);
            Assert.Contains("You have not reached the spend threshold for voucher YYY-YYY. Spend another £25.01 to receive £5.00 discount from your basket total.", basket.Messages.ToArray());
        }
    }
}
