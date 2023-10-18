using Asp.Versioning;
using Assignment_Project.Models;
using Assignment_Project.Models.Dto;
using Assignment_Project.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Assignment_Project.Controllers
{
    [Route("api/ApplicationUser")]
    [ApiController]
    [ApiVersionNeutral]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IApplicationUsersRepository _userRepo;
        private readonly ILogger<ApplicationUserController> _logger;

        public ApplicationUserController(IApplicationUsersRepository userRepo, ILogger<ApplicationUserController> logger)
        {
            _userRepo = userRepo;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginRequestDTO model)
        {
            try
            {
                var loginResponse = await _userRepo.Login(model);
                if (loginResponse.ApplicationUser == null || string.IsNullOrEmpty(loginResponse.Token))
                {
                    _logger.LogError("Login failed for user: {username}", model.UserName);
                    return BadRequest(new APIResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Username or password is incorrect" }
                    });
                }

                return Ok(new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Result = loginResponse
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login.");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<APIResponse>> Register([FromBody] RegisterationRequestDTO model)
        {
            try
            {
                bool isUserNameUnique = _userRepo.IsUniqueUser(model.UserName);
                if (!isUserNameUnique)
                {
                    _logger.LogWarning("Registration failed. Username already exists: {username}", model.UserName);
                    return BadRequest(new APIResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Username already exists" }
                    });
                }

                var user = await _userRepo.Register(model);
                if (user == null)
                {
                    _logger.LogError("Error while registering a user: {username}", model.UserName);
                    return BadRequest(new APIResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Error while registering" }
                    });
                }

                return Ok(new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration.");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
