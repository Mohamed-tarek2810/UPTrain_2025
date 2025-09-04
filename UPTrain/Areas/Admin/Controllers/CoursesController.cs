using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPTrain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using UPTrain.IRepositories;

namespace UPTrain.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoursesController : Controller
    {
        private readonly ICourseRepository _courseRepo;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Category> _categoryRepo;

        public CoursesController(ICourseRepository courseRepo, UserManager<User> userManager, IRepository<Category> categoryRepo)
        {
            _courseRepo = courseRepo;
            _userManager = userManager;
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _courseRepo.GetAllAsync(null,
                c => c. CreatedBy);
            return View(courses);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var users = await _userManager.Users.ToListAsync();
            var categories = await _categoryRepo.GetAllAsync(); 

            ViewBag.Users = new SelectList(users, "Id", "UserName");
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Courses course, IFormFile? ImageUrl)
        {
            if (!ModelState.IsValid)
            {
                var users = await _userManager.Users.ToListAsync();
                var categories = await _categoryRepo.GetAllAsync();

                ViewBag.Users = new SelectList(users, "Id", "UserName");
                ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");

                return View(course);
            }

            if (ImageUrl is not null && ImageUrl.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageUrl.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await ImageUrl.CopyToAsync(stream);
                }

                course.ImageUrl = "/images/" + fileName;
            }

            course.CreatedDate = DateTime.Now;

            await _courseRepo.AddAsync(course);
            await _courseRepo.CommitAsync();

            TempData["SuccessMessage"] = "Course created successfully!";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var course = await _courseRepo.GetOneAsync(c => c.CourseId == id);

            if (course is not null)
            {
                await _courseRepo.Delete(course);
                await _courseRepo.CommitAsync();

                TempData["SuccessMessage"] = "Course deleted successfully!";
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}
