using CamWebRtc.API.Models;
using CamWebRtc.Application.Interfaces;
using CamWebRtc.Infrastructure.Data;

namespace CamWebRtc.Application.Services
{
    public class StreamServices
    {
        private readonly AppDbContext _appDbContext;
        private readonly IStreamRepository _streamRepository;
        public StreamServices(AppDbContext appDbContext, IStreamRepository streamRepository)
        {
            _appDbContext = appDbContext;
            _streamRepository = streamRepository;
        }
        public string AddCam(StreamModel cam) => _streamRepository.AddCam(_appDbContext, cam);
        public List<StreamModel> GetCamsAll() => _streamRepository.GetCam(_appDbContext);
        public StreamModel GetCamId(int id) => _streamRepository.GetCamById(_appDbContext, id);
        public string UpdateCam(StreamModel cam) => _streamRepository.UpdateCam(_appDbContext, cam);
        public string DeletCamId(int id) => _streamRepository.DeleteCam(_appDbContext, id);
    }
}
