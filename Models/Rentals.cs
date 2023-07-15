namespace codeTestCom.Models
{
    public class Rentals
    {
        public Car Car { get; set; }
        public int NumOfContractedDays { get; set; }
        public int NumOfDaysUsed { get; set; }

        public Price? Price { get; set; }

        public Price CalculatePriceAndSurcharges()
        {
            decimal basePrice;
            decimal extraDayPrice;
            Price = new Price();

            switch(Car.Type)
            {
                case CarType.Premium:
                    basePrice = Utils.PREMIUM_PRICE;
                    extraDayPrice = Utils.PREMIUM_PRICE_EXTRA;
                    break;
                case CarType.Suv:
                    if(NumOfContractedDays <= Utils.FIRST_INTERVAL_DAYS)
                    {
                        basePrice = Utils.SUV_PRICE;
                    }
                    else if(NumOfContractedDays <= Utils.SECOND_INTERVAL_DAYS )
                    {
                        basePrice = Utils.SUV_PRICE * Utils.SUV_PRICE_SECOND_INTERVAL;
                    }
                    else
                    {
                        basePrice = Utils.SUV_PRICE * Utils.SUV_PRICE_THIRD_INTERVAL;
                    }

                    extraDayPrice = Utils.SUV_PRICE_EXTRA;
                    break;
                case CarType.Small:
                    if (NumOfContractedDays <= Utils.FIRST_INTERVAL_DAYS)
                    {
                        basePrice = Utils.SMALL_PRICE;
                    }
                    else
                    {
                        basePrice = Utils.SMALL_PRICE * Utils.SMALL_PRICE_SECOND_INTERVAL;
                    }

                    extraDayPrice = Utils.SMALL_PRICE_EXTRA;
                    break;
                default:
                    throw new NotImplementedException("Invalid car type.");
            }

            if(NumOfDaysUsed>NumOfContractedDays)
            {
                Price.Surcharges = basePrice * (NumOfDaysUsed - NumOfContractedDays) + basePrice * extraDayPrice * (NumOfDaysUsed - NumOfContractedDays);
            }

            Price.TotalPrice = basePrice * NumOfContractedDays + Price.Surcharges;

            return Price;
        }

        public Rentals(Car car, int numOfContractedDays, int numOfDaysUsed)
        {
            this.Car = car;
            this.NumOfContractedDays = numOfContractedDays;
            this.NumOfDaysUsed = numOfDaysUsed;
            CalculatePriceAndSurcharges();
        }

    }

    public class Price
    {
        public decimal TotalPrice { get; set; }
        public decimal Surcharges { get; set; }
    }
}
