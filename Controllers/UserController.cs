using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neues.Core.DTO;
using Neues.Core.Interface;
using Neues.Interface;
using Neues.Model;
using NeuesCore.Models;

namespace NeuesWebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUser _userSqlService;

        public UserController(ILogger<UserController> logger, IUser userSqlService)
        {
            _userSqlService = userSqlService;

        }


        [HttpGet]
        //[AllowAnonymous]
        [Route("~/user/all-users")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IEnumerable<User>> GetUsers()
        {

            try
            {
                return await _userSqlService.GetAllUsers();
            }
            catch (Exception e)
            {
                throw new Exception($"Conflict, {e.Message}");
            }

        }


        [HttpGet]
        //[Authorize]
        [Route("~/user/user-detail/{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<User> GetUser(Guid Id)
        {

            try
            {
                var user = await _userSqlService.GetUser(Id);
                //if (post.Image != null)
                //    post.Image = this.GetImage(Convert.ToBase64String(post.Image));
                return user;

            }
            catch (Exception e)
            {
                throw new Exception($"Conflict, {e.Message}");
            }

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("~/user/create")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<User> AddUser([FromBody] UserDto userDto)
        {
            try
            {
                var Allusers = await _userSqlService.GetAllUsers();
                var userEmail = Allusers.FirstOrDefault(c => c.Email == userDto.Email);
                if (userEmail != null)
                    throw new Exception("User already exist");

                if (userDto.Password != userDto.ReEnterPassword)
                    throw new Exception("Wrong credentials");

                User user = new User();
                user.FullName = userDto.FullName;
                user.UserName = userDto.UserName;
                user.Email = userDto.Email;
                user.Password = userDto.Password;
                //user.ReEnterPassword = userDto.ReEnterPassword;
                user.Country = userDto.Country;

                
                await _userSqlService.CreateUser(user);
                return user;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }


        [HttpPut]
        [Authorize]
        [Route("~/user/update")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO UpdateUserDto)
        {
            try
            {
                User user = new User();
                user.Id = UpdateUserDto.Id;
                user.FullName = UpdateUserDto.FullName;
                user.Password = UpdateUserDto.Password;
                user.Country = UpdateUserDto.Country;

                await _userSqlService.UpdateUser(user);
                return Ok(user);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        [HttpDelete]
        [Route("~/user/delete/{Id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(Guid Id)
        {
            try
            {
                await _userSqlService.DeleteUser(Id);
                return Ok(Id);
            }
            catch (Exception e)
            {

                throw new Exception($"Post not found, {e.Message}");
            }
        }
    }
}
