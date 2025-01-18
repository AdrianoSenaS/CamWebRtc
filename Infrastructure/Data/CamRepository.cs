using CamWebRtc.API.Models;
using CamWebRtc.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CamWebRtc.Infrastructure.Data
{
    public class CamRepository : ICamRepository
    {
        public string AddCam(AppDbContext apiDb, CamModel cam)
        {
            apiDb.Cams.Add(cam);
            apiDb.SaveChanges();
            return "OK";
        }
        public CamModel GetCamById(AppDbContext apiDb, int id)
        {

            CamModel? cam = apiDb.Cams.FirstOrDefault(c => c.Id == id);
            return cam;
        }
        public List<CamModel> GetCam(AppDbContext apiDb)
        {
            List<CamModel> cam = apiDb.Cams.ToList();
            return cam;
        }
        public string UpdateCam(AppDbContext apiDb, CamModel cam)
        {
            CamModel? cams = apiDb.Cams.FirstOrDefault(c => c.Id == cam.Id);
            if (cams != null)
            {
                apiDb.Entry(cams).State = EntityState.Detached;
            }
            apiDb.Cams.Update(cam);
            apiDb.SaveChanges();
            return "Ok";
        }
        public string DeleteCam(AppDbContext apiDb, int Id)
        {
            CamModel? cams = apiDb.Cams.FirstOrDefault(c => c.Id == Id);
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
