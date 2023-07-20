using codeTestCom;
using codeTestCom.Models;
using Microsoft.Azure.Cosmos;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace codeTestComTDD
{
    public class codeTestComUnitTest
    {
        [Fact]
        public void CalculatePriceAndSurchargesPremiumExactDaysTest()
        {
            Car car = new Car("0000AAA", "BMW 7", "BMW", CarType.Premium);

            Rental rental = new Rental(new RentalRQ(car.Id, car.Type, "10/08/2023", "20/08/2023", "5334369R"));


            Assert.Equal(3000m, rental.Price.BasePrice);
            Assert.Equal(0m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesPremiumExtraDaysTest()
        {
            Car car = new Car("0000AAA", "BMW 7", "BMW", CarType.Premium);
            Rental rental = new Rental(car.Id, car.Type, car.PartitionKey, Utils.ConvertStringToDateTime("10/08/2023"), Utils.ConvertStringToDateTime("20/08/2023"), "5334369R");
            rental.CalculatePriceAndSurcharges("22/08/2023");

            Assert.Equal(3000m, rental.Price.BasePrice);
            Assert.Equal(720m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSuvFirstIntervalExactDaysTest()
        {
            Car car = new Car("1111AAA", "Nissan Juke", "Nissan", CarType.Suv);
            Rental rental = new Rental(car.Id, car.Type, car.PartitionKey, Utils.ConvertStringToDateTime("10/08/2023"), Utils.ConvertStringToDateTime("12/08/2023"), "5334369R");
            rental.CalculatePrice();

            Assert.Equal(300m, rental.Price.BasePrice);
            Assert.Equal(0m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSuvFirstIntervalExtraDaysTest()
        {
            Car car = new Car("1111AAA", "Nissan Juke", "Nissan", CarType.Suv);
            Rental rental = new Rental(car.Id, car.Type, car.PartitionKey, Utils.ConvertStringToDateTime("10/08/2023"), Utils.ConvertStringToDateTime("12/08/2023"), "5334369R");
            rental.CalculatePriceAndSurcharges("13/08/2023");

            Assert.Equal(300m, rental.Price.BasePrice);
            Assert.Equal(240m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSuvSecondIntervalExactDaysTest()
        {
            Car car = new Car("1111AAA", "Nissan Juke", "Nissan", CarType.Suv);
            Rental rental = new Rental(car.Id, car.Type, car.PartitionKey, Utils.ConvertStringToDateTime("10/08/2023"), Utils.ConvertStringToDateTime("22/08/2023"), "5334369R");
            rental.CalculatePrice();

            Assert.Equal(1440m, rental.Price.BasePrice);
            Assert.Equal(0m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSuvSecondIntervalExtraDaysTest()
        {
            Car car = new Car("1111AAA", "Nissan Juke", "Nissan", CarType.Suv);
            Rental rental = new Rental(car.Id, car.Type, car.PartitionKey, Utils.ConvertStringToDateTime("10/08/2023"), Utils.ConvertStringToDateTime("22/08/2023"), "5334369R");
            rental.CalculatePriceAndSurcharges("19/09/2023");

            Assert.Equal(1440m, rental.Price.BasePrice);
            Assert.Equal(5376m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSuvThirdIntervalExactDaysTest()
        {
            Car car = new Car("1111AAA", "Nissan Juke", "Nissan", CarType.Suv);
            Rental rental = new Rental(car.Id, car.Type, car.PartitionKey, Utils.ConvertStringToDateTime("10/08/2023"), Utils.ConvertStringToDateTime("19/09/2023"), "5334369R");
            rental.CalculatePrice();

            Assert.Equal(3000m, rental.Price.BasePrice);
            Assert.Equal(0m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSuvThirddIntervalExtraDaysTest()
        {
            Car car = new Car("1111AAA", "Nissan Juke", "Nissan", CarType.Suv);
            Rental rental = new Rental(car.Id, car.Type, car.PartitionKey, Utils.ConvertStringToDateTime("10/08/2023"), Utils.ConvertStringToDateTime("19/09/2023"), "5334369R");
            rental.CalculatePriceAndSurcharges("04/10/2023");

            Assert.Equal(3000m, rental.Price.BasePrice);
            Assert.Equal(1800m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSmallFirstIntervalExactDaysTest()
        {
            Car car = new Car("2222AAA", "Skoda Fabia", "Skoda", CarType.Small);
            Rental rental = new Rental(car.Id, car.Type, car.PartitionKey, Utils.ConvertStringToDateTime("10/08/2023"), Utils.ConvertStringToDateTime("15/08/2023"), "5334369R");
            rental.CalculatePrice();

            Assert.Equal(250m, rental.Price.BasePrice);
            Assert.Equal(0m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSmallFirstIntervalExtraDaysTest()
        {
            Car car = new Car("2222AAA", "Skoda Fabia", "Skoda", CarType.Small);
            Rental rental = new Rental(car.Id, car.Type, car.PartitionKey, Utils.ConvertStringToDateTime("10/08/2023"), Utils.ConvertStringToDateTime("15/08/2023"), "5334369R");
            rental.CalculatePriceAndSurcharges("22/08/2023");

            Assert.Equal(250m, rental.Price.BasePrice);
            Assert.Equal(455m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSmallSecondIntervalExactDaysTest()
        {
            Car car = new Car("2222AAA", "Skoda Fabia", "Skoda", CarType.Small);
            Rental rental = new Rental(car.Id, car.Type, car.PartitionKey, Utils.ConvertStringToDateTime("10/08/2023"), Utils.ConvertStringToDateTime("09/09/2023"), "5334369R");
            rental.CalculatePrice();

            Assert.Equal(900m, rental.Price.BasePrice);
            Assert.Equal(0m, rental.Price.Surcharges);
        }

        [Fact]
        public void CalculatePriceAndSurchargesSmallSecondIntervalExtraDaysTest()
        {
            Car car = new Car("2222AAA", "Skoda Fabia", "Skoda", CarType.Small);
            Rental rental = new Rental(car.Id, car.Type, car.PartitionKey, Utils.ConvertStringToDateTime("10/08/2023"), Utils.ConvertStringToDateTime("09/09/2023"), "5334369R");
            rental.CalculatePriceAndSurcharges("19/09/2023");

            Assert.Equal(900m, rental.Price.BasePrice);
            Assert.Equal(390m, rental.Price.Surcharges);
        }
    }
}