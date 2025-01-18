using CamWebRtc.API.Models;
using CamWebRtc.Application.Interfaces;
using CamWebRtc.Infrastructure.Data;

namespace CamWebRtc.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _appDbContext;
        public UserService(IUserRepository userRepository, AppDbContext appDbContext)
        {
            _userRepository = userRepository;
            _appDbContext = appDbContext;
        }
        public List<UserModel> GetUsersAll() => _userRepository.GetUser(_appDbContext);
        public UserModel GetUserId(int id) => _userRepository.GetUserById(_appDbContext,id);
        public string AddUser(UserModel user) => _userRepository.AddUser(_appDbContext, user);
        public string Updateuser(UserModel user)=>_userRepository.UpdateUser(_appDbContext, user);
        public string DeleteUser(int id) =>_userRepository.DeleteUser(_appDbContext, id);
    }
}
