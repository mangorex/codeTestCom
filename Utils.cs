using codeTestCom.Models;
using System.Globalization;

namespace codeTestCom
{
    public class Utils
    {
        #region Constants

        #region Prices
        public const decimal PREMIUM_PRICE = 300m;
        public const decimal PREMIUM_PRICE_EXTRA = 0.2m;

        public const decimal SUV_PRICE = 150m;
        public const decimal SUV_PRICE_SECOND_INTERVAL = 0.8m;
        public const decimal SUV_PRICE_THIRD_INTERVAL = 0.5m;
        public const decimal SUV_PRICE_EXTRA = 0.6m;

        public const decimal SMALL_PRICE = 50m;
        public const decimal SMALL_PRICE_SECOND_INTERVAL = 0.6m;
        public const decimal SMALL_PRICE_EXTRA = 0.3m;

        #endregion

        #region DAYS_INTERVALS
        public const int FIRST_INTERVAL_DAYS = 7;
        public const int SECOND_INTERVAL_DAYS = 30;
        #endregion

        public const string ERROR_CAR_RENTED = "The car had already been rented";
        public const string DATABASE_ID = "RentalDB";
        public const string CONTAINER_CAR_ID = "Cars";
        public const string CONTAINER_RENTAL_ID = "Rentals";
        public const string CONTAINER_USER_ID = "Users";
        public const int LOYALTY_PREMIUM = 5;
        public const int LOYALTY_SUV = 3;
        public const int LOYALTY_SMALL = 1;

        public const string FORMAT_DATE = "dd/MM/yyyy";

        #region COMMON_METHODS
        public static int CalculateLoyaltyPoints(CarType carType)
        {
            int loyaltyPoints;

            switch (carType)
            {
                case CarType.Premium:
                    loyaltyPoints = Utils.LOYALTY_PREMIUM;
                    break;
                case CarType.Suv:
                    loyaltyPoints = Utils.LOYALTY_SUV;
                    break;
                case CarType.Small:
                    loyaltyPoints = Utils.LOYALTY_SMALL;
                    break;
                default:
                    throw new NotImplementedException("Invalid car type.");
            }

            return loyaltyPoints;
        }

        public static DateTime ConvertStringToDateTime(string dateString)
        {
            DateTime dateTime;
            DateTime.TryParseExact(dateString, FORMAT_DATE, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
            return dateTime;
        }
        #endregion
        #endregion

    }
}
