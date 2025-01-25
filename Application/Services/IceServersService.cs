using CamWebRtc.API.Models;
using CamWebRtc.Application.Interfaces;
using CamWebRtc.Infrastructure.Data;

namespace CamWebRtc.Application.Services
{
    public class IceServersService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IiceServersRepository _iceServersRepository;
        public IceServersService(AppDbContext appDbContext, IiceServersRepository iiceServersRepository) 
        {
            _appDbContext = appDbContext;
            _iceServersRepository = iiceServersRepository;
        }
        public string AddIceServes(IceServersModel iceServers) => _iceServersRepository.AddIceServers(_appDbContext, iceServers);
        public List<IceServersModel> GetIceServersAll() => _iceServersRepository.GetIceServersAll(_appDbContext);
        public IceServersModel GetIceServersId(int id)=> _iceServersRepository.GetIceServers(_appDbContext, id);
        public string UpdateIceServers(IceServersModel iceServers) => _iceServersRepository.UpdateIceServes(_appDbContext, iceServers);
        public string DeleteIceServes(int id) => _iceServersRepository.DeleteIceServer(_appDbContext, id);
    }
}
