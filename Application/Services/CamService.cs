using CamWebRtc.API.Models;
using CamWebRtc.Application.Interfaces;
using CamWebRtc.Infrastructure.Data;

namespace CamWebRtc.Application.Services
{
    public class CamService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICamRepository _camRepository;

        public CamService(AppDbContext appDbContext, ICamRepository camRepository)
        {
            _appDbContext = appDbContext;
            _camRepository = camRepository;
        }

        public string AddCam(CamModel cam) => _camRepository.AddCam(_appDbContext, cam);
        public List<CamModel> GetCamsAll() => _camRepository.GetCam(_appDbContext);
        public CamModel GetCamId(int id) => _camRepository.GetCamById(_appDbContext, id);
        public string UpdateCam(CamModel cam) => _camRepository.UpdateCam(_appDbContext, cam);
        public string DeletCamId(int id) => _camRepository.DeleteCam(_appDbContext, id);
    }
}
