using codeTestCom.Models;

namespace codeTestCom.Repository
{
    public interface IRentalRepository
    {
        Task<Rental> CreateRentalAsync(Rental rental);
        Task<Rental> GetRentalAsyncById(string id);
        Task<Rental> GetRentalAsyncByCarId(string carId);
        Task<Rental> UpdateRentalAsync(Rental rental, DateTime actualReturnDate);
    }
}
