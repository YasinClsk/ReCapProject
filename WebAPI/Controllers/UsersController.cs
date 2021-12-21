using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Security.Hashing;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _userService.GetAll();

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("update")]
        public IActionResult Update(UserForUpdateDto userForUpdate)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userForUpdate.Password, out passwordHash, out passwordSalt);

            var user = new User
            {
                Id = userForUpdate.Id,
                FirstName = userForUpdate.FirstName,
                LastName = userForUpdate.LastName,
                Email = userForUpdate.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
            };
            var result = _userService.Update(user);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("delete")]
        public IActionResult Delete(User user)
        {
            
            var result = _userService.Delete(user);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("getuserbyid")]
        public IActionResult GetUserById(int id)
        {
            var result = _userService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
