using System;
using NUnit.Framework;

namespace PGShoppingBasket.Test
{
    [TestFixture]
    public class BasketTests
    {
        // Scenario: Basket 1
        [Test]
        public void GivenItemsInBasket_WhenApplyGiftVoucher_ThenTotalIsCorrect()
        {
            Assert.Fail();
        }

        // Scenario: Basket 2
        [Test]
        public void GivenItemsInBasket_WhenApplyOfferVoucherNotReachingThreshold_ThenTotalIsCorrectAndMessageDisplayed()
        {
            Assert.Fail();
        }

        // Scenario: Basket 3
        [Test]
        public void GivenItemsInBasket_WhenApplyOfferVoucherReachingThreshold_ThenDiscountDoesNotExceedQualifyingItemPriceAndTotalIsCorrect()
        {
            Assert.Fail();
        }

        // Scenario: Basket 4
        [Test]
        public void GivenItemsInBasket_WhenApplyGiftVoucherAndApplyOfferVoucher_ThenTotalIsCorrect()
        {
            Assert.Fail();
        }

        // Scenario: Basket 5
        [Test]
        public void GivenItemsInBasketIncludingGiftVoucher_WhenApplyOfferVoucherNotReachingThreshold_ThenTotalIsCorrectAndMessageDisplayed()
        {
            Assert.Fail();
        }
    }
}
