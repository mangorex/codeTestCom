using codeTestCom.Models;

namespace codeTestCom.Repository
{
    public interface IRentalRepository
    {
        Task<Rental> CreateRentalAsync(Rental rental);
        Task<Rental> GetRentalAsync(string id);

    }
}
