using UPTrain.Data;
using UPTrain.IRepositories;
using UPTrain.Models;

namespace UPTrain.Repositories
{
    public class PointRepository : Repository<Point>, IPointRepository
    {
        public PointRepository(ApplicationDbContext context) : base(context) { }
    }
}
