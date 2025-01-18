using CamWebRtc.API.Models;
using CamWebRtc.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CamWebRtc.Infrastructure.Data
{
    public class UserRepository: IUserRepository
    {

        public string AddUser(AppDbContext apiDb, UserModel user)
        {
            apiDb.Users.Add(user);
            apiDb.SaveChanges();
            return "OK";
        }
        public UserModel GetUserById(AppDbContext apiDb, int id)
        {
          
            UserModel ?user = apiDb.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }
        public List<UserModel> GetUser(AppDbContext apiDb)
        {
            List<UserModel> users = apiDb.Users.ToList();
            return users;
        }
        public string UpdateUser(AppDbContext apiDb, UserModel user)
        {
            UserModel ?users = apiDb.Users.FirstOrDefault(u => u.Id == user.Id);
            if (users != null) 
            {
               apiDb.Entry(users).State = EntityState.Detached;
            }
            apiDb.Users.Update(user);
            apiDb.SaveChanges();
            return "Ok";
        }
        public string DeleteUser(AppDbContext apiDb, int Id)
        {
            UserModel ?users = apiDb.Users.FirstOrDefault(u => u.Id == Id);
            if (users != null)
            {
                apiDb.Remove(users);
                apiDb.SaveChanges();
                return "OK";
            }
            return "Error";
        }

    }
}
