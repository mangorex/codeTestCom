using codeTestCom.Models;
using codeTestCom.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Collections.Concurrent;
using System.Net;
using User = codeTestCom.Models.User;

namespace codeTestCom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IUserRepository _userRepository;

        public RentalController(ICarRepository carRepository, IRentalRepository rentalRepository, IUserRepository userRepository)
        {
            _carRepository = carRepository;
            _rentalRepository = rentalRepository;
            _userRepository = userRepository;
        }

        [HttpGet("CalculatePrice")]
        public ActionResult<Price> CalculatePrice(Rental rental)
        {
            return rental.CalculatePrice();
        }

        [HttpGet("CalculatePriceAndSurcharges")]
        public async Task<ActionResult<Price>> CalculatePriceAndSurcharges(string id, int numOfDaysUsed)
        {
            Rental rentalDB = await _rentalRepository.GetRentalAsyncById(id);

            if(string.IsNullOrWhiteSpace(rentalDB.Id))
            {
                return null;
            }

            return rentalDB.CalculatePriceAndSurcharges(numOfDaysUsed);
        }

        [HttpPost("RentCar")]
        public async Task<ActionResult<Rental>> RentCar(Rental rental)
        {
            if(rental.CarId == null)
            {
                return NotFound("Car Id not found.");
            }
            Car car = await _carRepository.GetCarAsyncById(rental.CarId);
            User user = await _userRepository.GetUserAsyncByDni(rental.UserId);

            if (car == null)
            {
                return NotFound("Car not found.");
            }

            if (car.IsRented)
            {
                return Conflict("The car has already been rented.");
            }

            car.IsRented = true;

            car = await _carRepository.UpdateCarAsync(car, true);

            if(car.IsRented)
            {
                Rental rentalDB = new Rental(car, rental.NumOfContractedDays, user.Dni);

                rental = await _rentalRepository.CreateRentalAsync(rentalDB);
                await _userRepository.UpdateUserLoyaltyAsync(user, Utils.CalculateLoyaltyPoints(car.Type));
            }
            return rental;
        }



        [HttpPost("RentMultipleCar")]
        public async Task<ActionResult> RentMultipleCar([FromBody] RentMultipleCarRequest request)
        {
            List<object> dataList = new List<object>();
            Rental rental = request.Rental;
            User user = await _userRepository.GetUserAsyncByDni(rental.UserId);
            decimal totalPrice = 0;

            foreach (string carId in request.CarIds)
            {
                Car car = await _carRepository.GetCarAsyncById(carId);

                if (car != null && !car.IsRented)
                {
                    car.IsRented = true;

                    car = await _carRepository.UpdateCarAsync(car, true);

                    if (car.IsRented)
                    {
                        Rental rentalDB = new Rental(car, rental.NumOfContractedDays, user.Dni);

                        rental = await _rentalRepository.CreateRentalAsync(rentalDB);
                        await _userRepository.UpdateUserLoyaltyAsync(user, Utils.CalculateLoyaltyPoints(car.Type));
                        dataList.Add(new {Precio = rental.Price.BasePrice, Car = rental.CarId });
                        totalPrice += rental.Price.BasePrice;
                    }
                }
            }

            var infoList = new
            {
                Dni = user.Dni,
                TotalPrice = totalPrice,
                DataList = dataList
            };

            return Ok(infoList);
        }

        [HttpPost("ReturnCar")]
        public async Task<ActionResult<Rental>> ReturnCar(string carId, int numOfDaysUsed)
        {
            Car car = await _carRepository.GetCarAsyncById(carId);

            if (car == null)
            {
                return NotFound("Car not found.");
            }

            if (!car.IsRented)
            {
                return Conflict("The car has not been rented.");
            }

            car = await _carRepository.UpdateCarAsync(car, false);
            Rental rental = await _rentalRepository.GetRentalAsyncByCarId(carId);
            rental.CalculatePriceAndSurcharges(numOfDaysUsed);
            return rental;
        }
    }
}