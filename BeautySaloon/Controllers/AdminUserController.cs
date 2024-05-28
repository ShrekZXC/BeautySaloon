using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.Exception;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
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
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public AdminUserController(
            IUserService userService, 
            IRoleService roleService,
            ILogger<AdminUserController> logger,
            IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsers();
            var userViewModels = _mapper.Map<List<UserViewModel>>(users);

            return View("~/Views/Admin/User/Index.cshtml", userViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var userViewModel = new UserViewModel();
            var roles = await _roleService.GetAllRoles();
            userViewModel.Roles = _mapper.Map<List<RoleViewModel>>(roles);

            return View("~/Views/Admin/User/Add.cshtml", userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<UserModel>(userViewModel);
            
                var result = await _userService.RegisterUserAsync(user, userViewModel.Password, userViewModel.SelectedRole, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "AdminUser");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            var roles = await _roleService.GetAllRoles();
            userViewModel.Roles = _mapper.Map<List<RoleViewModel>>(roles);
            return View("~/Views/Admin/User/Add.cshtml", userViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(Guid id)
        {
            var user = await _userService.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _roleService.GetAllRoles();
            
            var userViewModel = _mapper.Map<UserViewModel>(user);
            userViewModel.Roles = _mapper.Map<List<RoleViewModel>>(roles);
            userViewModel.SelectedRole = await _roleService.GetSelectedRole(id);

            return View("~/Views/Admin/User/Update.cshtml", userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserViewModel userViewModel)
        {
            var userModel = _mapper.Map<UserModel>(userViewModel);
            var result = await _userService.UpdateUser(userModel);
            
            if (result == null)
            {
                return NotFound();
            }
            
            if (result.Succeeded)
            {
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
            var result = await _userService.DeleteUser(id);
            if (result == null)
            {
                return NotFound();
            }
            
            if (result.Succeeded)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, errors = result.Errors });
        }
    }
}
