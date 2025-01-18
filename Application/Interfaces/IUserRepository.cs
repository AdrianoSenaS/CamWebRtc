using CamWebRtc.API.Models;
using CamWebRtc.Infrastructure.Data;

namespace CamWebRtc.Application.Interfaces
{
    public interface IUserRepository
    {
        string AddUser(AppDbContext apiDb, UserModel user);
        List<UserModel> GetUser(AppDbContext apiDb);
        UserModel GetUserById(AppDbContext apiDb, int id);
        string UpdateUser(AppDbContext apiDb, UserModel user);
        string DeleteUser(AppDbContext apiDb, int Id);
    }
}
