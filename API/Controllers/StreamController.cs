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
    public class StreamController : ControllerBase
    {
        private readonly StreamServices _streamServices;
        
        public StreamController(StreamServices streamServices)
        {
            _streamServices = streamServices;
        }
        // GET: api/<CamController>
        [HttpGet]
        public List<StreamModel> Get()
        {
            return _streamServices.GetCamsAll();
        }

        // GET api/<CamController>/5
        [HttpGet("{id}")]
        public StreamModel Get(int id)
        {
            return _streamServices.GetCamId(id);
        }

        // POST api/<CamController>
        [HttpPost]
        public void Post(StreamModel cam)
        {
            _streamServices.AddCam(cam);
        }

        // PUT api/<CamController>/5
        [HttpPut]
        public void Put(StreamModel cam)
        {
            _streamServices.UpdateCam(cam);
        }

        // DELETE api/<CamController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _streamServices.DeletCamId(id);
        }
    }
}
