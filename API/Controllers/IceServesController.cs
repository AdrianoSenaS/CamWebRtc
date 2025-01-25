using CamWebRtc.API.Models;
using CamWebRtc.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CamWebRtc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IceServesController : ControllerBase
    {
        private readonly IceServersService _iceServersService;
        public IceServesController(IceServersService iceServers) 
        {
            _iceServersService = iceServers;
        }
        // GET: api/<IceServesController>
        [HttpGet]
        public  List<IceServersModel> GetAll()
        {
            return _iceServersService.GetIceServersAll();
        }

        // GET api/<IceServesController>/5
        [HttpGet("{id}")]
        public IceServersModel Get(int id)
        {
            return _iceServersService.GetIceServersId(id);
        }

        // POST api/<IceServesController>
        [HttpPost]
        public string Post(IceServersModel iceServers)
        {
            return _iceServersService.AddIceServes(iceServers);
        }

        // PUT api/<IceServesController>/5
        [HttpPut("{id}")]
        public string Put(IceServersModel iceServers)
        {
            return _iceServersService.UpdateIceServers(iceServers);
        }

        // DELETE api/<IceServesController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            return _iceServersService.DeleteIceServes(id);
        }
    }
}
