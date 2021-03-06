using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobsonDev.Authentication.Models;
using RobsonDev.Authentication.Services;
using RobsonDev.Common;
using RobsonDev.Data.Repositories;
using RobsonDev.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobsonDev.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthenticateController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserViewModel))]
        public async Task<ActionResult> LoginAsync([FromBody] User user)
        {
            var userFind = await _userRepository.FindAsync(user);            

            if (userFind == null) return Unauthorized(new { message = "Unauthorized access. Invalid user or password." });

            if(!PasswordCryptoHelper.VerifyPassword(user.Password, userFind.Password)) return Unauthorized(new { message = "Unauthorized access. Invalid user or password." });

            var token = TokenService.GenerateToken(userFind);

            userFind.Password = "";

            return Ok(new UserViewModel { User = userFind, Token = token });
        }

        //[HttpPost]
        //public async Task<ActionResult> PostAsync([FromBody] User user)
        //{
        //    var us =  await _userRepository.Insert(user).ConfigureAwait(false);
        //    return Created("",us);
        //}

        //[HttpPut]
        //public async Task<ActionResult> PutAsync([FromBody] User user)
        //{
        //    await _userRepository.Update(user);
        //    return Ok();
        //}
    }
}
