using CamWebRtc.API.Models;
using CamWebRtc.Infrastructure.Data;

namespace CamWebRtc.Application.Interfaces
{
    public interface ICamRepository
    {
        string AddCam(AppDbContext apiDb, CamModel cam);
        List<CamModel> GetCam(AppDbContext apiDb);
        CamModel GetCamById(AppDbContext apiDb, int id);
        string UpdateCam(AppDbContext apiDb, CamModel cam);
        string DeleteCam(AppDbContext apiDb, int Id);
    }
}
