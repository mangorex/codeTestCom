using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace codeTestCom.Models
{
    public class Rental
    {
        [JsonProperty("id")]
        public string? Id { get; set; }
        [JsonProperty(PropertyName = "partitionKey")]
        public string? PartitionKey { get; set; }
        public string CarId { get; set; }
        public int NumOfContractedDays { get; set; }

        public int NumOfDaysUsed { get; set; }

        public Price? Price { get; set; }
        public CarType CarType { get; set; }
        public bool IsCarReturned { get; set; }

        [JsonConstructor]
        public Rental()
        {
            // Constructor sin parámetros o inicialización personalizada
            this.IsCarReturned = false;
        }

        public Rental(Car car, int numOfContractedDays)
        {
            this.Id = Guid.NewGuid().ToString();
            this.CarId = car.Id;
            this.CarType = car.Type;
            this.PartitionKey = car.PartitionKey + "#" + numOfContractedDays.ToString();
            this.NumOfContractedDays = numOfContractedDays;
            this.IsCarReturned = false;
        }

        public Price CalculatePrice()
        {
            decimal dayBasePrice;
            Price = new Price();

            switch (CarType)
            {
                case CarType.Premium:
                    dayBasePrice = Utils.PREMIUM_PRICE;
                    break;
                case CarType.Suv:
                    if (NumOfContractedDays <= Utils.FIRST_INTERVAL_DAYS)
                    {
                        dayBasePrice = Utils.SUV_PRICE;
                    }
                    else if (NumOfContractedDays <= Utils.SECOND_INTERVAL_DAYS)
                    {
                        dayBasePrice = Utils.SUV_PRICE * Utils.SUV_PRICE_SECOND_INTERVAL;
                    }
                    else
                    {
                        dayBasePrice = Utils.SUV_PRICE * Utils.SUV_PRICE_THIRD_INTERVAL;
                    }

                    break;
                case CarType.Small:
                    if (NumOfContractedDays <= Utils.FIRST_INTERVAL_DAYS)
                    {
                        dayBasePrice = Utils.SMALL_PRICE;
                    }
                    else
                    {
                        dayBasePrice = Utils.SMALL_PRICE * Utils.SMALL_PRICE_SECOND_INTERVAL;
                    }

                    break;
                default:
                    throw new NotImplementedException("Invalid car type.");
            }

            decimal basePrice = dayBasePrice * NumOfContractedDays;

            Price = new Price()
            {
                BasePrice = basePrice,
                TotalPrice = basePrice
            };
            return Price;
        }

        public Price CalculatePriceAndSurcharges(int numOfDaysUsed)
        {
            decimal basePricePerDay;
            decimal extraDayPrice;

            basePricePerDay = Price.BasePrice / this.NumOfContractedDays;
            this.NumOfDaysUsed = numOfDaysUsed;

            switch (CarType)
            {
                case CarType.Premium:
                    extraDayPrice = Utils.PREMIUM_PRICE_EXTRA;
                    break;
                case CarType.Suv:

                    extraDayPrice = Utils.SUV_PRICE_EXTRA;
                    break;
                case CarType.Small:
                    extraDayPrice = Utils.SMALL_PRICE_EXTRA;
                    break;
                default:
                    throw new NotImplementedException("Invalid car type.");
            }

            if (NumOfDaysUsed > NumOfContractedDays)
            {
                int extraDays = (NumOfDaysUsed - NumOfContractedDays);
                Price.Surcharges = basePricePerDay * extraDays + basePricePerDay * extraDays * extraDayPrice;
            }

            this.Price.TotalPrice = this.Price.BasePrice + this.Price.Surcharges;
            return Price;
        }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Price
    {
        public decimal BasePrice { get; set; }
        public decimal Surcharges { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
