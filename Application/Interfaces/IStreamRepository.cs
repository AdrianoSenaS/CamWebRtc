using CamWebRtc.API.Models;
using CamWebRtc.Infrastructure.Data;

namespace CamWebRtc.Application.Interfaces
{
    public interface IStreamRepository
    {
        string AddCam(AppDbContext apiDb, StreamModel cam);
        List<StreamModel> GetCam(AppDbContext apiDb);
        StreamModel GetCamById(AppDbContext apiDb, int id);
        string UpdateCam(AppDbContext apiDb, StreamModel cam);
        string DeleteCam(AppDbContext apiDb, int Id);
    }
}
