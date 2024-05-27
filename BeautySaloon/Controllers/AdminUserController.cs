using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.Exception;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private readonly ILogger<AdminUserController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AdminUserController(
            ILogger<AdminUserController> logger,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userViewModels = _mapper.Map<List<UserViewModel>>(users);

            return View("~/Views/Admin/User/Index.cshtml", userViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var userViewModel = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Roles = _mapper.Map<List<RoleViewModel>>(_roleManager.Roles.ToList())
            };

            return View("~/Views/Admin/User/Add.cshtml", userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserViewModel userViewModel)
        {
            try
            {
                // TODO: Вынести в бизнес логику
                var user = new ApplicationUser
                {
                    Id = userViewModel.Id,
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email,
                    FirstName = userViewModel.FirstName,
                    SecondName = userViewModel.SecondName,
                    LastName = userViewModel.LastName
                };

                var result = await _userManager.CreateAsync(user, userViewModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, userViewModel.SelectedRole);
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            catch (DuplicateEmailException)
            {
                ModelState.TryAddModelError("Email", "Email уже существует");
            }

            return View("~/Views/Admin/User/Add.cshtml", userViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var userViewModel = _mapper.Map<UserViewModel>(user);
            userViewModel.Roles = _mapper.Map<List<RoleViewModel>>(_roleManager.Roles.ToList());

            return View("~/Views/Admin/User/Update.cshtml", userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserViewModel userViewModel)
        {
            var user = await _userManager.FindByIdAsync(userViewModel.Id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            user.Email = userViewModel.Email;
            user.FirstName = userViewModel.FirstName;
            user.SecondName = userViewModel.SecondName;
            user.LastName = userViewModel.LastName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.AddToRoleAsync(user, userViewModel.SelectedRole);

                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("~/Views/Admin/User/Update.cshtml", userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, errors = result.Errors });
        }
    }
}
