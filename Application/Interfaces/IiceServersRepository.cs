using CamWebRtc.API.Models;
using CamWebRtc.Infrastructure.Data;

namespace CamWebRtc.Application.Interfaces
{
    public interface IiceServersRepository
    {
        public string AddIceServers(AppDbContext appDb, IceServersModel iceServers);
        public List<IceServersModel> GetIceServersAll(AppDbContext appDb);
        public IceServersModel GetIceServers(AppDbContext appDb, int id);
        public string UpdateIceServes(AppDbContext appDb, IceServersModel iceServers);
        public string DeleteIceServer(AppDbContext appDb, int id);
    }
}
