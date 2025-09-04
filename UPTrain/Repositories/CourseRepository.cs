using UPTrain.Data;
using UPTrain.IRepositories;
using UPTrain.Models;

namespace UPTrain.Repositories
{
    public class CourseRepository : Repository<Courses>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context) { }
    }
}
