using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnvilTotalPriceCalcDyn;
using System;
using SUT = AnvilTotalPriceCalcDyn;
using System.Collections.Generic;

namespace AnvilTotalPriceCalcDyn.UnitTests
{
    [TestClass]
    public class DynCalculationTests
    {
        //Dynamic Test Data Source For Price Per Anvil 
        private static IEnumerable<object[]> GetDiscountPriceTestData()
        {
            return new List<object[]>()
            {
                new object[] { 0, 20.00, (Func<int, double, double>)CalculationLib.CalcPricePerAnvil, 0.00 },
                new object[] { 0, 50.00, (Func<int, double, double>)CalculationLib.CalcPricePerAnvil, 0.00 },
                new object[] { 1, 20.00, (Func<int, double, double>)CalculationLib.CalcPricePerAnvil, 20.00 },
                new object[] { 1, 50.00, (Func<int, double, double>)CalculationLib.CalcPricePerAnvil, 50.00 },
                new object[] { 10, 20.00, (Func<int, double, double>)CalculationLib.CalcPricePerAnvil, 18.00 },
                new object[] { 10, 50.00, (Func<int, double, double>)CalculationLib.CalcPricePerAnvil, 45.00 },
                new object[] { 21, 20.00, (Func<int, double, double>)CalculationLib.CalcPricePerAnvil, 16.00 },
                new object[] { 21, 50.00, (Func<int, double, double>)CalculationLib.CalcPricePerAnvil, 40.00 }
            };
        }

        //Dynamic Test Data Source For Price Per Anvil With Negative Quantity - Test for exception
        private static IEnumerable<object[]> DiscountPriceExceptionData()
        {
            return new List<object[]>()
            {
                new object[] { -1, 20.00, (Func<int, double, double>)CalculationLib.CalcPricePerAnvil, 0.00 },
                new object[] { -1, 50.00, (Func<int, double, double>)CalculationLib.CalcPricePerAnvil, 0.00 },
            };
        }


        //Dynamic Test Data Source For Shipping Cost By Zone
        private static IEnumerable<object[]> GetShippingTestData()
        {
            return new List<object[]>()
            {
                new object[] { 1, (Func<int, double>)CalculationLib.CalcShippingCostPerAnvil, 10.00 },
                new object[] { 2, (Func<int, double>)CalculationLib.CalcShippingCostPerAnvil, 20.00 },
                new object[] { 3, (Func<int, double>)CalculationLib.CalcShippingCostPerAnvil, 30.00 }
            };
        }

        //Dynamic Test Data Source For Shipping Cost By Zone With Invalid Zone - Test for exception
        private static IEnumerable<object[]> GetShippingFailureTestData()
        {
            return new List<object[]>()
            {
                new object[] { 0, (Func<int, double>)CalculationLib.CalcShippingCostPerAnvil, 10.00 },
                new object[] { 0, (Func<int, double>)CalculationLib.CalcShippingCostPerAnvil, 20.00 },
                new object[] { 0, (Func<int, double>)CalculationLib.CalcShippingCostPerAnvil, 30.00 },
                new object[] { 4, (Func<int, double>)CalculationLib.CalcShippingCostPerAnvil, 10.00 },
                new object[] { 4, (Func<int, double>)CalculationLib.CalcShippingCostPerAnvil, 20.00 },
                new object[] { 4, (Func<int, double>)CalculationLib.CalcShippingCostPerAnvil, 30.00 }
            };
        }

        [DynamicData(nameof(GetDiscountPriceTestData), DynamicDataSourceType.Method)]
        [TestMethod]
        public void CalcPricePerAnvil_EachTierRepresented_ShouldEqualResult(int quantity, double regPrice, Func<int, double, double> perAnvilCalc, double result)
        {
            var actual = perAnvilCalc(quantity, regPrice);

            Assert.AreEqual(result, actual);
        }

        [DynamicData(nameof(DiscountPriceExceptionData), DynamicDataSourceType.Method)]
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CalcPricePerAnvil_NegQuantity_ShouldThrowException(int quantity, double regPrice, Func<int, double, double> perAnvilCalc, double result)
        {
            var actual = perAnvilCalc(quantity, regPrice);

            Assert.AreEqual(result, actual);
        }

        [DynamicData(nameof(GetShippingTestData), DynamicDataSourceType.Method)]
        [TestMethod]
        public void CalcShippingCostPerAnvil_Zones1To3_ShouldEqualResult(int zone, Func<int, double> shippingPerAnvilCalc, double result)
        {
            var actual = shippingPerAnvilCalc(zone);

            Assert.AreEqual(result, actual);
        }

        [DynamicData(nameof(GetShippingFailureTestData), DynamicDataSourceType.Method)]
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CalcShippingCostPerAnvil_InvalidZones_0And4(int zone, Func<int, double> shippingPerAnvilCalc, double result)
        {
            var actual = shippingPerAnvilCalc(zone);

            Assert.AreEqual(result, actual);
        }
    }
}
