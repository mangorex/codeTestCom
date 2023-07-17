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
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("GetUserByDni")]
        public async Task<ActionResult<User>> GetUserByDni(string dni)
        {
            return await _userRepository.GetUserAsyncByDni(dni);
        }

        [HttpPost("AddUser")]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            try
            {
                var createdUser = await _userRepository.AddUserAsync(user);
                return Ok(createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}