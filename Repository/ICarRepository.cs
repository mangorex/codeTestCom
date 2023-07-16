﻿using codeTestCom.Models;

namespace codeTestCom.Repository
{
    public interface ICarRepository
    {
        Task<Car> GetCarAsync(string carId);
        Task<Car> UpdateCarAsync(Car car, bool rented);
    }
}