using codeTestCom.Models;
using Microsoft.AspNetCore.Mvc;

namespace codeTestCom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalController : ControllerBase
    {

        [HttpPost("CalculatePrice")]
        public ActionResult<Price> CalculatePrice(Rentals rental)
        {
            return rental.CalculatePrice();
        }

        [HttpPost("CalculatePriceAndSurcharges")]
        public ActionResult<Price> CalculatePriceAndSurcharges(Rentals rental)
        {
           return rental.CalculatePriceAndSurcharges(rental.NumOfDaysUsed);
        }
    }
}