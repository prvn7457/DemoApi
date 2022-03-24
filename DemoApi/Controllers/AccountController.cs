using DemoApi.Contracts.Request;
using DemoApi.Contracts.Response;
using DemoApi.Repository.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IUser userService;

        private IConfiguration _config { get; }

        public AccountController(IUser users, IConfiguration config)
        {
            userService = users;
            _config = config;
        }
        [HttpPost]
        [Route("SignIn")]
        public IActionResult SingIn(SignInModel model)
        {
            if (model != null)
            {
                var user = userService.SignIn(model);
                var apiResponse = new ApiResponse();
                if (user == null)
                {
                    //User not found
                    apiResponse.Ok = false;
                    apiResponse.Status = 404;
                    apiResponse.Message = "Invalid Login Creadential!";
                    return Ok(apiResponse);
                }
                else
                {
                    //Success login
                    string token = GenerateJSONWebToken();
                    apiResponse.Ok = true;
                    apiResponse.Status = 200;
                    apiResponse.Message = "Login Success !";
                    apiResponse.Data = user;
                    apiResponse.Token = token;
                    return Ok(apiResponse);
                }
            }
            else
            {
                return BadRequest();
            }
        }

       [HttpPost]
        [Route("SignUp")]
        public IActionResult SignUp(SignUpModel model)
        {
            if (model != null)
            {
                var user = userService.SignUp(model);
                var apiResponse = new ApiResponse();
               
                {
                    //Success login
                    apiResponse.Ok = true;
                    apiResponse.Status = 200;
                    apiResponse.Message = "User created Successfully !";
                    apiResponse.Data = user;
                    
                    return Ok(apiResponse);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
