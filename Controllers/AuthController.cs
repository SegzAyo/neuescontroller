using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Neues.Core.DTO;
using Neues.Core.Interface;
using Neues.Core.Models;
using Neues.Infrastructure.Email;
using NeuesCore.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NeuesWebApp.Controllers
{


    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUser _userSqlService;

        public AuthController(ILogger<AuthController> logger, IUser userSqlService)
        {
            _userSqlService = userSqlService;

        }


        /*[HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPassword password)
        {
            if (ModelState.IsValid)
            {
                var user = await _userSqlService.ResetPassword(password);
                if (user != null)
                {
                    var token = await _userSqlService.GeneratePasswordResetToken(user);

                    var passwordResetLink = await Url.Action("ResetPassword", "Account", new { email = password.Email, token = token }, Request.Scheme);

                    logger.Log(LogLevel.Warning, passwordResetLink);

                    return Ok("ForgotPasswordConfirmation");
                }
                return Ok("ForgotPasswordConfirmation");
            }
            return Ok(password);
        }*/

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO != null && !string.IsNullOrWhiteSpace(loginDTO.Email) && !string.IsNullOrWhiteSpace(loginDTO.Password))
            {
                var users = await _userSqlService.GetAllUsers();
                if (users != null)
                {
                    var user = users.FirstOrDefault(x => x.Email == loginDTO.Email && x.Password == loginDTO.Password);
                    if (user != null)
                    {
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.FullName),
                     new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, "Guest", "Admin")
                };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SOME_RANDOM_KEY_DO_NOT_SHARE"));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var expires = DateTime.Now.AddDays(30);

                        var token = new JwtSecurityToken(
                           "http://yourdomain.com",
                            "http://yourdomain.com",
                            claims,
                            expires: expires,
                            signingCredentials: creds
                        );

                        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                        //return Ok(new { Token = tokenString });
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    return Unauthorized();
                }


            }
            else
            {
                return BadRequest("Invalid client request");
            }
        }

        [HttpPost]
        [Route("~/auth/user/forgotpassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var isUserExist = await _userSqlService.IsUserExist(email);

            if (isUserExist)
            {

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, email)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SOME_RANDOM_KEY_DO_NOT_SHARE"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddDays(30);

                var token = new JwtSecurityToken(
                   "https://neueswatch-api.azurewebsites.net",
                    "https://neueswatch-api.azurewebsites.net",
                    claims,
                    expires: expires,
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                string baseUrl = "https://neueswatch-api.azurewebsites.net";
                //Create URL with above token
                var lnkHref = $"<a href='{baseUrl}?email={email}&token={tokenString}'>Reset Password</a>";
                //HTML Template for Send email
                string subject = "Your changed password";
                string body = "<b>Please find the Password Reset Link. </b><br/>" + lnkHref;
                //Get and set the AppSettings using configuration manager.

                //Call send email methods.
                try
                {
                    EmailManager.SendEmail(subject, body, email);
                    return Ok("Email Sent");
                }
                catch (Exception ex)
                {
                    return Ok($"Email not Sent{ex}");
                }

            }

            return Ok("User not found");
        }

        [HttpPost]
        [Route("~/auth/user/resetpassword")]
        public async Task<IActionResult> ResetPassword(string email, string newPassword, string token)
        {
            if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(newPassword) && !string.IsNullOrWhiteSpace(token))
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                    var claimValue = securityToken.Claims.FirstOrDefault(c => c.Value == email)?.Value;
                    if (claimValue == email)
                    {
                        var isUpdated = await _userSqlService.UpdateUserPassword(email, newPassword);
                        if (isUpdated)
                        {
                            return Ok("Password Updated");
                        }
                    }

                }
                catch (Exception)
                {
                    //TODO: Logger.Error
                    return BadRequest("Error");
                }
            }
            return BadRequest("Bad Request");
        }


    }
}

