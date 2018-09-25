using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        public readonly UserService _userService = new UserService();

        [HttpGet]
        //[Route("api/users")]
        public IHttpActionResult Get()
        {
            var content = _userService.Users.Select(x => new { x.Id, x.Name, x.Username });
            return Ok(content);
        }
    }
}
