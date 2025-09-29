using UPTrain.Data;
using UPTrain.IRepositories;
using UPTrain.Models;

namespace UPTrain.Repositories
{
    public class LessonCompletionsRepository : Repository<LessonCompletion>, ILessonCompletionsRepository
    {

        public LessonCompletionsRepository(ApplicationDbContext context) : base(context) { }
    }
}
