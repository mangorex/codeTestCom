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
        public ActionResult<Price> CalculatePrice(RentalRQ rentalRQ)
        {
            Rental rental = new Rental(rentalRQ);
            return rental.CalculatePrice();
        }

        [HttpGet("CalculatePriceAndSurcharges")]
        public async Task<ActionResult<Price>> CalculatePriceAndSurcharges(string carId, string actualReturnDate)
        {
            Rental rentalDB = await _rentalRepository.GetRentalAsyncByCarId(carId);

            if (string.IsNullOrWhiteSpace(rentalDB.Id))
            {
                return null;
            }

            return rentalDB.CalculatePriceAndSurcharges(actualReturnDate);
        }

        [HttpPost("RentCar")]
        public async Task<ActionResult<Rental>> RentCar(RentalRQ rentalRQ)
        {
            Rental rental = new Rental();

            if (rentalRQ.CarId == null)
            {
                return NotFound("Car Id not found.");
            }
            Car car = await _carRepository.GetCarAsyncById(rentalRQ.CarId);
            User user = await _userRepository.GetUserAsyncByDni(rentalRQ.UserId);

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

            if (car.IsRented)
            {
                Rental rentalDB = new Rental(rentalRQ, car);
                rental = await _rentalRepository.CreateRentalAsync(rentalDB);
                await _userRepository.UpdateUserLoyaltyAsync(user, Utils.CalculateLoyaltyPoints(car.Type));
            }
            return rental;
        }

        [HttpPost("RentMultipleCar")]
        public async Task<ActionResult> RentMultipleCar([FromBody] RentMultipleCarRequest request)
        {
            List<object> dataList = new List<object>();
            RentalRQ rentalRQ = request.RentalRQ;
            Rental rental = new Rental();

            User user = await _userRepository.GetUserAsyncByDni(rentalRQ.UserId);
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

                        Rental rentalDB = new Rental(rentalRQ, car);

                        rental = await _rentalRepository.CreateRentalAsync(rentalDB);
                        await _userRepository.UpdateUserLoyaltyAsync(user, Utils.CalculateLoyaltyPoints(car.Type));
                        dataList.Add(new { Precio = rental.Price.BasePrice, Car = rental.CarId });
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
        public async Task<ActionResult<Rental>> ReturnCar(string carId, string actualReturnDate)
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
            Rental rental = await _rentalRepository.GetRentalAsyncByCarId(car.Id);
            rental.CalculatePriceAndSurcharges(actualReturnDate);
            await _rentalRepository.UpdateRentalAsync(rental, (DateTime)rental.ActualReturnDate);

            return rental;
        }
    }
}