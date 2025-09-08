using UPTrain.Models;

namespace UPTrain.IRepositories
{
    public interface ICourseRepository : IRepository<Courses>
    {
        Task<Courses?> GetByIdAsync(int id);
    }
}
