using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UPTrain.IRepositories;
using UPTrain.Models;

namespace UPTrain.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CoursesController : Controller
    {
        private readonly ICourseRepository _courseRepository;

        public CoursesController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        [Authorize]
        public async Task<IActionResult> Courses()
        {
            var courses = await _courseRepository.GetAllAsync();
            return View(courses);
        }

        public async Task<IActionResult> CoursesDetails(int id)
        {
            var course = await _courseRepository.GetOneAsync(
                c => c.CourseId == id,
                c => c.Lessons,
                c => c.Quizzes,
                c => c.CreatedBy
            );

            if (course == null)
                return NotFound();

            return View(course);
        }


        
        public async Task<IActionResult> CourseQuizzes(int id)
        {
            var course = await _courseRepository.GetAllAsync(
                c => c.CourseId == id,
                c => c.Quizzes,           
                c => c.Quizzes.Select(q => q.Questions) 
            );

            if (course == null)
                return NotFound();

            return View(course);
        }
    }
}
