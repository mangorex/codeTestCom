using codeTestCom;
using codeTestCom.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace codeTestComTDD
{
    public class codeTestComUnitTest
    {
        [Fact]
        public void CalculatePriceAndSurchargesPremiumExactDaysTest()
        {
            Car car = new Car("0000AAA", "BMW 7", "BMW", CarType.Premium);
            Rental rental = new Rental(car, 10);

            Assert.Equal(3000m, rental.Price.BasePrice);
            Assert.Equal(0m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesPremiumExtraDaysTest()
        {
            Car car =new Car("0000AAA", "BMW 7", "BMW", CarType.Premium);
            Rental rental = new Rental(car, 10);
            rental.CalculatePriceAndSurcharges(12);

            Assert.Equal(3000m, rental.Price.BasePrice);
            Assert.Equal(720m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSuvFirstIntervalExactDaysTest()
        {
            Car car = new Car("1111AAA", "Nissan Juke", "Nissan", CarType.Suv);
            Rental rental = new Rental( car, 2);

            Assert.Equal(300m, rental.Price.BasePrice);
            Assert.Equal(0m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSuvFirstIntervalExtraDaysTest()
        {
            Car car = new Car("1111AAA", "Nissan Juke", "Nissan", CarType.Suv);
            Rental rental = new Rental(car, 2);
            rental.CalculatePriceAndSurcharges(3);

            Assert.Equal(300m, rental.Price.BasePrice);
            Assert.Equal(240m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSuvSecondIntervalExactDaysTest()
        {
            Car car = new Car("1111AAA", "Nissan Juke", "Nissan", CarType.Suv);
            Rental rental = new Rental(car, 12);

            Assert.Equal(1440m, rental.Price.BasePrice);
            Assert.Equal(0m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSuvSecondIntervalExtraDaysTest()
        {
            Car car = new Car("1111AAA", "Nissan Juke", "Nissan", CarType.Suv);
            Rental rental = new Rental(car, 12);
            rental.CalculatePriceAndSurcharges(40);

            Assert.Equal(1440m, rental.Price.BasePrice);
            Assert.Equal(5376m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSuvThirdIntervalExactDaysTest()
        {
            Car car = new Car("1111AAA", "Nissan Juke", "Nissan", CarType.Suv);
            Rental rental = new Rental(car, 40);

            Assert.Equal(3000m, rental.Price.BasePrice);
            Assert.Equal(0m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSuvThirddIntervalExtraDaysTest()
        {
            Car car = new Car("1111AAA", "Nissan Juke", "Nissan", CarType.Suv);
            Rental rental = new Rental(car, 40);
            rental.CalculatePriceAndSurcharges(55);

            Assert.Equal(3000m, rental.Price.BasePrice);
            Assert.Equal(1800m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSmallFirstIntervalExactDaysTest()
        {
            Car car = new Car("2222AAA", "Skoda Fabia", "Skoda", CarType.Small);
            Rental rental = new Rental(car, 5);

            Assert.Equal(250m, rental.Price.BasePrice);
            Assert.Equal(0m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSmallFirstIntervalExtraDaysTest()
        {
            Car car = new Car("2222AAA", "Skoda Fabia", "Skoda", CarType.Small);
            Rental rental = new Rental(car, 5);
            rental.CalculatePriceAndSurcharges(12);

            Assert.Equal(250m, rental.Price.BasePrice);
            Assert.Equal(455m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSmallSecondIntervalExactDaysTest()
        {
            Car car = new Car("2222AAA", "Skoda Fabia", "Skoda", CarType.Small);
            Rental rental = new Rental(car, 30);

            Assert.Equal(900m, rental.Price.BasePrice);
            Assert.Equal(0m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSmallSecondIntervalExtraDaysTest()
        {
            Car car = new Car("2222AAA", "Skoda Fabia", "Skoda", CarType.Small);
            Rental rental = new Rental(car, 30);
            rental.CalculatePriceAndSurcharges(40);

            Assert.Equal(900m, rental.Price.BasePrice);
            Assert.Equal(390m, rental.Price.Surcharges);
        }
    }
}