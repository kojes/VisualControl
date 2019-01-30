using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using VueExample.Models;
using VueExample.Providers;
using VueExample.ResponseObjects;

namespace VueExample.Controllers
{
    [Authorize]
    [ApiController]
    [Route("auth/[controller]")]
    public class UserController : Controller
    {
        private IUserProvider _userProvider;

        public UserController(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User user)
        {
            var currentUser = _userProvider.Authenticate(user.Username, user.Password);

            if (currentUser == null)
                return BadRequest(new StandardResponseObject{ Message = "Пароль или имя пользователя не совпадают" });

            return Ok(new StandardResponseObject<User>{Body = currentUser, Message = "Успешная авторизация"});
        }

        
    }
}
