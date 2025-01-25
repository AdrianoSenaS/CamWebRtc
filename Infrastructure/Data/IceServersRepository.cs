using CamWebRtc.API.Models;
using CamWebRtc.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CamWebRtc.Infrastructure.Data
{
    public class IceServersRepository : IiceServersRepository
    {
        public string AddIceServers(AppDbContext appDb, IceServersModel iceServers)
        {
            appDb.IceServers.Add(iceServers);
            appDb.SaveChanges();
            return "OK";
        }
        public List<IceServersModel> GetIceServersAll(AppDbContext appDb)
        {
            List<IceServersModel>? iceServers = appDb.IceServers
                .Include(i=>i.UrlsStun)
                .Include(i=>i.urlsTurn)
                .ToList();
            return iceServers;
        }
        public IceServersModel GetIceServers(AppDbContext appDb, int id)
        {
            IceServersModel ?iceServers = appDb.IceServers
                 .Include(i => i.UrlsStun)
                .Include(i => i.urlsTurn)
                .FirstOrDefault(ice => ice.Id == id);
            return iceServers;
        }
        public string UpdateIceServes(AppDbContext appDb, IceServersModel iceServers)
        {
            IceServersModel ?iceServer = appDb.IceServers
                 .Include(i => i.UrlsStun)
                .Include(i => i.urlsTurn)
                .FirstOrDefault(ice => ice.Id == iceServers.Id);
            if (iceServer != null)
            {
                appDb.Entry(iceServer).State = EntityState.Detached;
            }
            appDb.Update(iceServers);
            appDb.SaveChanges();
            return "OK";
        }
        public string DeleteIceServer(AppDbContext appDb, int id)
        {
            IceServersModel ?iceServers = appDb.IceServers
                .Include(i => i.UrlsStun)
                .Include(i => i.urlsTurn)
                .FirstOrDefault(i => i.Id == id);
            if (iceServers != null)
            {
                appDb.Entry(iceServers).State = EntityState.Detached;
            }
            appDb.Remove(iceServers);
            appDb.SaveChanges();
            return "OK";
        }
    }
}
