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

        
        public async Task<IActionResult> Courses(string searchQuery, int pageNumber = 1, int pageSize = 20)
        {
            var courses = await _courseRepository.GetAllAsync(includes: [e => e.CreatedBy, e => e.Category]);

            
            if (!string.IsNullOrEmpty(searchQuery))
            {
                courses = courses.Where(c =>
                    c.Title.Contains(searchQuery) ||
                    (c.Category != null && c.Category.Name.Contains(searchQuery))
                ).ToList();
            }

           
            var totalCourses = courses.Count();
            var totalPages = (int)Math.Ceiling(totalCourses / (double)pageSize);

            var paginatedCourses = courses
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchQuery = searchQuery;

            return View(paginatedCourses);
        }

        // 📌 تفاصيل الكورس
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

        // 📌 الكويزات المرتبطة بالكورس
        public async Task<IActionResult> CourseQuizzes(int id)
        {
            var course = await _courseRepository.GetOneAsync(
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
