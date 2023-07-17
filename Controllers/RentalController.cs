using codeTestCom.Models;
using codeTestCom.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Collections.Concurrent;
using System.Net;

namespace codeTestCom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly IRentalRepository _rentalRepository;

        public RentalController(ICarRepository carRepository, IRentalRepository rentalRepository)
        {
            _carRepository = carRepository;
            _rentalRepository = rentalRepository;
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
            Car car = await _carRepository.GetCarAsyncById(rental.CarId);
            
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
                Rental rentalDB = new Rental(car, rental.NumOfContractedDays);

                rental = await _rentalRepository.CreateRentalAsync(rentalDB);
            }
            return rental;
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