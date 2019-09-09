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

        private readonly Product _expensiveHatProduct;
        private readonly Product _cheapHatProduct;
        private readonly Product _expensiveJumperProduct;
        private readonly Product _cheapJumperProduct;
        private readonly Product _headLightProduct;
        private readonly Product _thirtyPoundGiftVoucherProduct;

        private readonly GiftVoucher _fivePoundGiftVoucher;

        private readonly OfferVoucher _headGearOfferVoucher;
        private readonly OfferVoucher _fiveOffFiftyOfferVoucher;

        public BasketTests()
        {
            _customer = new Customer();
            _headGearCategory = new ProductCategory("Head Gear");
            _giftVoucherCategory = new ProductCategory("Gift Vouchers", true);

            _expensiveHatProduct = new Product("Hat", 25.00m, 1);
            _cheapHatProduct = new Product("Hat", 10.50m, 1);
            _expensiveJumperProduct = new Product("Jumper", 54.65m, 1);
            _cheapJumperProduct = new Product("Jumper", 26.00m, 1);
            _headLightProduct = new Product("Head Light", 3.50m, 1, _headGearCategory);
            _thirtyPoundGiftVoucherProduct = new Product("£30 Gift Voucher ", 30.00m, 1, _giftVoucherCategory);

            _fivePoundGiftVoucher = new GiftVoucher("XXX-XXX", 5.00m);

            _headGearOfferVoucher = new OfferVoucher("YYY-YYY", 5.00m, 50.00m, _headGearCategory);
            _fiveOffFiftyOfferVoucher = new OfferVoucher("YYY-YYY", 5.00m, 50.00m);
        }

        /// <summary>
        /// Basket 1:
        /// 1 Hat @ £10.50
        /// 1 Jumper @ £54.65
        /// ------------
        /// 1 x £5.00 Gift Voucher XXX-XXX applied
        /// ------------
        /// Total: £60.15
        /// </summary>
        [Test]
        public void Basket1_GivenItemsInBasket_WhenApplyGiftVoucher_ThenTotalIsCorrect()
        {
            // Arrange
            var basket = new Basket(_customer);

            // Act
            basket.AddProduct(_cheapHatProduct);
            basket.AddProduct(_expensiveJumperProduct);
            basket.RedeemGiftVoucher(_fivePoundGiftVoucher);

            TestContext.Write(basket.Overview());

            // Assert
            Assert.AreEqual(60.15, basket.GetTotal());
            Assert.IsEmpty(basket.Messages);
        }
        /// <summary>
        /// Basket 2:
        /// 1 Hat @ £25.00
        /// 1 Jumper @ £26.00
        /// ------------
        /// 1 x £5.00 off Head Gear in baskets over £50.00 Offer Voucher YYY-YYY applied
        /// ------------
        /// Total: £51.00
        /// Message: “There are no products in your basket applicable to voucher Voucher YYY-YYY.”
        /// </summary>
        [Test]
        public void Basket2_GivenItemsInBasket_WhenApplyOfferVoucherNotReachingThreshold_ThenTotalIsCorrectAndMessageDisplayed()
        {
            // Arrange
            var basket = new Basket(_customer);

            // Act
            basket.AddProduct(_expensiveHatProduct);
            basket.AddProduct(_cheapJumperProduct);
            basket.ApplyOfferVoucher(_headGearOfferVoucher);

            TestContext.Write(basket.Overview());

            // Assert
            Assert.AreEqual(51.00, basket.GetTotal());
            Assert.Contains("There are no products in your basket applicable to voucher Voucher YYY-YYY", basket.Messages.ToArray());
        }

        /// <summary>
        /// Basket 3:
        /// 1 Hat @ £25.00
        /// 1 Jumper @ £26.00
        /// 1 Head Light(Head Gear Category of Product)  @ £3.50
        ///     ------------
        ///     1 x £5.00 off Head Gear in baskets over £50.00 Offer Voucher YYY-YYY applied
        /// ------------
        /// Total: £51.00
        /// </summary>
        [Test]
        public void Basket3_GivenItemsInBasket_WhenApplyOfferVoucherReachingThreshold_ThenDiscountDoesNotExceedQualifyingItemPriceAndTotalIsCorrect()
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
            Assert.AreEqual(51.00, basket.GetTotal());
            Assert.IsEmpty(basket.Messages);
        }

        /// <summary>
        /// Basket 4:
        /// 1 Hat @ £25.00
        /// 1 Jumper @ £26.00
        /// ------------
        /// 1 x £5.00 Gift Voucher XXX-XXX applied
        /// 1 x £5.00 off baskets over £50.00 Offer Voucher YYY-YYY applied
        /// ------------
        /// Total: £41.00
        /// </summary>
        [Test]
        public void Basket4_GivenItemsInBasket_WhenApplyGiftVoucherAndApplyOfferVoucher_ThenTotalIsCorrect()
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
            Assert.AreEqual(41.00, basket.GetTotal());
            Assert.IsEmpty(basket.Messages);
        }

        /// <summary>
        /// Basket 5:
        /// 1 Hat @ £25.00
        /// 1 £30 Gift Voucher @ £30.00
        /// ------------
        /// 1 x £5.00 off baskets over £50.00 Offer Voucher YYY-YYY applied
        /// ------------
        /// Total: £55.00
        /// ------------
        /// Message: “You have not reached the spend threshold for voucher YYY-YYY.Spend another £25.01 to receive £5.00 discount from your basket total.”
        /// </summary>
        [Test]
        public void Basket5_GivenItemsInBasketIncludingGiftVoucher_WhenApplyOfferVoucherNotReachingThreshold_ThenTotalIsCorrectAndMessageDisplayed()
        {
            // Arrange
            var basket = new Basket(_customer);

            // Act
            basket.AddProduct(_expensiveHatProduct);
            basket.AddProduct(_thirtyPoundGiftVoucherProduct);
            basket.ApplyOfferVoucher(_fiveOffFiftyOfferVoucher);

            TestContext.Write(basket.Overview());

            // Assert
            Assert.AreEqual(55.00, basket.GetTotal());
            Assert.Contains("You have not reached the spend threshold for voucher YYY-YYY. Spend another £25.01 to receive £5.00 discount from your basket total.", basket.Messages.ToArray());
        }
    }
}
