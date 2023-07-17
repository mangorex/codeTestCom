using codeTestCom.Models;

namespace codeTestCom.Repository
{
    public interface ICarRepository
    {
        Task<Car> GetCarAsyncById(string id);
        Task<Car> UpdateCarAsync(Car car, bool rented);
        Task<Car> AddCarAsync(Car car);
    }
}
