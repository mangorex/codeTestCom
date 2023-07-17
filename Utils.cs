using codeTestCom.Models;

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

        #endregion

    }
}
