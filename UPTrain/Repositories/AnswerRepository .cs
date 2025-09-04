using UPTrain.Data;
using UPTrain.IRepositories;
using UPTrain.Models;

namespace UPTrain.Repositories
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        public AnswerRepository(ApplicationDbContext context) : base(context) { }
    }
}
