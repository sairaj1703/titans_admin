using Microsoft.AspNetCore.Mvc;
using titans_admin.Services.Interfaces;
using titans_admin.Models.ViewModels;

namespace titans_admin.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public UsersController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _adminService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _adminService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var (success, errorMessage, userId) = await _adminService.CreateUserAsync(model);

            if (!success)
                return BadRequest(new { error = errorMessage ?? "Failed to create user" });

            // Return the created user representation
            var createdUser = await _adminService.GetUserByIdAsync(userId ?? 0);
            return CreatedAtAction(nameof(Get), new { id = userId }, createdUser);
        }
    }
}
