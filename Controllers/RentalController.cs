using codeTestCom.Models;
using Microsoft.AspNetCore.Mvc;

namespace codeTestCom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalController : ControllerBase
    {

        [HttpPost("CalculatePrice")]
        public ActionResult<Price> CalculatePrice(Rental rental)
        {
            return rental.CalculatePrice();
        }

        [HttpPost("CalculatePriceAndSurcharges")]
        public ActionResult<Price> CalculatePriceAndSurcharges(Rental rental)
        {
           return rental.CalculatePriceAndSurcharges(rental.NumOfDaysUsed);
        }
    }
}