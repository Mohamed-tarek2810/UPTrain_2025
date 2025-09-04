using UPTrain.Data;
using UPTrain.IRepositories;
using UPTrain.Models;

namespace UPTrain.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }
    }
}
