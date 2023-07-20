//using Newtonsoft.Json;
//using System;
//using System.ComponentModel.DataAnnotations;

namespace codeTestCom.Models
{
    public class RentalRQ
    {
        public string? CarId { get; set; }

        public CarType? CarType { get; set; }
        public string ContractDeliveryDate { get; set; }

        public string ContractReturnDate { get; set; }
        public string? ActualReturnDate { get; set; }

        public string UserId { get; set; }

        public RentalRQ(string? carId, CarType? carType, string contractDeliveryDate, string contractReturnDate, string userId)
        {
            CarId = carId;
            CarType = carType;
            ContractDeliveryDate = contractDeliveryDate;
            ContractReturnDate = contractReturnDate;
            UserId = userId;
        }

    }

}
