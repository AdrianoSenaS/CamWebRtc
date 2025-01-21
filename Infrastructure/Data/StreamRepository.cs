using CamWebRtc.API.Models;
using CamWebRtc.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CamWebRtc.Infrastructure.Data
{
    public class StreamRepository : IStreamRepository
    {
        public string AddCam(AppDbContext apiDb, StreamModel cam)
        {
            apiDb.Streams.Add(cam);
            apiDb.SaveChanges();
            return "OK";
        }
        public StreamModel GetCamById(AppDbContext apiDb, int id)
        {

            StreamModel? cam = apiDb.Streams.FirstOrDefault(c => c.Id == id);
            return cam;
        }
        public List<StreamModel> GetCam(AppDbContext apiDb)
        {
            List<StreamModel> cam = apiDb.Streams.ToList();
            return cam;
        }
        public string UpdateCam(AppDbContext apiDb, StreamModel cam)
        {
            StreamModel? cams = apiDb.Streams.FirstOrDefault(c => c.Id == cam.Id);
            if (cams != null)
            {
                apiDb.Entry(cams).State = EntityState.Detached;
            }
            apiDb.Streams.Update(cam);
            apiDb.SaveChanges();
            return "Ok";
        }
        public string DeleteCam(AppDbContext apiDb, int Id)
        {
            StreamModel? cams = apiDb.Streams.FirstOrDefault(c => c.Id == Id);
            if (cams != null)
            {
                apiDb.Remove(cams);
                apiDb.SaveChanges();
                return "OK";
            }
            return "Error";
        }
    }
}
