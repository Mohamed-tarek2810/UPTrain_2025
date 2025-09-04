using Microsoft.AspNetCore.Mvc;
using UPTrain.IRepositories;
using UPTrain.Models;

namespace UPTrain.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IRepository<User> _userRepo;

        public UsersController(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userRepo.GetAllAsync();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                await _userRepo.AddAsync(user);
                await _userRepo.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userRepo.GetOneAsync(u => u.Id == id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, User user)
        {
            if (id != user.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(user);

            var existingUser = await _userRepo.GetOneAsync(u => u.Id == id);
            if (existingUser == null)
                return NotFound();

            // ✅ تحديث الحقول المطلوبة فقط
            existingUser.FullName = user.FullName;
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;

            await _userRepo.CommitAsync();
            TempData["SuccessMessage"] = "User updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userRepo.GetOneAsync(u => u.Id == id);

            if (user is not null)
            {
                await _userRepo.Delete(user);
                await _userRepo.CommitAsync();

                TempData["SuccessMessage"] = "User deleted successfully!";
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var user = await _userRepo.GetOneAsync(c => c.Id == id);

            if (user is not null)
            {
                await _userRepo.Delete(user);
                await _userRepo.CommitAsync();

                TempData["SuccessMessage"] = "User deleted successfully!";
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Block(string id)
        {
            var user = await _userRepo.GetOneAsync(u => u.Id == id);
            if (user == null) return NotFound();

            user.IsBlocked = true;
            await _userRepo.CommitAsync();

            TempData["SuccessMessage"] = $"User {user.FullName} has been blocked!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Unblock(string id)
        {
            var user = await _userRepo.GetOneAsync(u => u.Id == id);
            if (user == null) return NotFound();

            user.IsBlocked = false;
            await _userRepo.CommitAsync();

            TempData["SuccessMessage"] = $"User {user.FullName} has been unblocked!";
            return RedirectToAction(nameof(Index));
        }
    }
}
