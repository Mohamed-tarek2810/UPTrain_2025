using Microsoft.EntityFrameworkCore;
using UPTrain.Data;
using UPTrain.IRepositories;
using UPTrain.Models;

namespace UPTrain.Repositories
{
    public class CourseRepository : Repository<Courses>, ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Courses?> GetByIdAsync(int id)
        {
            return await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseId == id);
        }
    }
}
