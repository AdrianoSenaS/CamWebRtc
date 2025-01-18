using CamWebRtc.API.Models;
using CamWebRtc.Application.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CamWebRtc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        // GET: api/<UserController>
        [HttpGet]
        public List<UserModel> Get()
        {
            return _userService.GetUsersAll();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public UserModel Get(int id)
        {
            return _userService.GetUserId(id);
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post(UserModel user)
        {
            _userService.AddUser(user);
        }

        // PUT api/<UserController>/5
        [HttpPut]
        public void Put(UserModel user)
        {
            _userService.Updateuser(user);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _userService.DeleteUser(id);
        }
    }
}
