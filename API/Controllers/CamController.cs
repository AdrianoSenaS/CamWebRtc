using CamWebRtc.API.Models;
using CamWebRtc.Application.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CamWebRtc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CamController : ControllerBase
    {
         private readonly CamService _camService;
        public CamController(CamService camService)
        {
            _camService = camService;
        }
        
        // GET: api/<CamController>
        [HttpGet]
        public List<CamModel> Get()
        {
            return _camService.GetCamsAll(); ;
        }

        // GET api/<CamController>/5
        [HttpGet("{id}")]
        public CamModel Get(int id)
        {
            return _camService.GetCamId(id);
        }

        // POST api/<CamController>
        [HttpPost]
        public void Post(CamModel cam)
        {
            _camService.AddCam(cam);
        }

        // PUT api/<CamController>/5
        [HttpPut]
        public void Put(CamModel cam)
        {
            _camService.UpdateCam(cam);
        }

        // DELETE api/<CamController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _camService.DeletCamId(id);
        }
    }
}
