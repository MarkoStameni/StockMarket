using StockMarket.Server.Models;
using StockMarket.Server.Models.Response;
using StockMarket.Server.Models.Responses;
using StockMarket.Server.Requests.User;
using StockMarket.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace StockMarket.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("GetUser/{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _userService.GetAsync(userId);

            if (user == null)
                return Ok(new ApiResult<BaseResponse>(new BaseResponse { Message = "No users found" }));

            return Ok(new ApiResult<UserResponse>(user));
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetListAsync();

            if (users.Count == 0)
                return Ok(new ApiResult<BaseResponse>(new BaseResponse { Message = "No users found" }));

            return Ok(new ApiResult<List<UserResponse>>(users));
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] AddUserRequest addUserRequest)
        {
            if (string.IsNullOrEmpty(addUserRequest.FirstName))
            {
                Log.Warning("Failed creating user, no user first name");
                return BadRequest(new ApiResult(ErrorCodes.MissingParameters, "Empty user FirstName"));
            }

            if (string.IsNullOrEmpty(addUserRequest.LastName))
            {
                Log.Warning("Failed creating user, no user last name");
                return BadRequest(new ApiResult(ErrorCodes.MissingParameters, "Empty user LastName"));
            }

            if (string.IsNullOrEmpty(addUserRequest.Email))
            {
                Log.Warning("Failed creating user, no user email");
                return BadRequest(new ApiResult(ErrorCodes.MissingParameters, "Empty user Email"));
            }

            if (string.IsNullOrEmpty(addUserRequest.Password))
            {
                Log.Warning("Failed creating user, no user Password");
                return BadRequest(new ApiResult(ErrorCodes.MissingParameters, "Empty user Password"));
            }

            var newUserId = await _userService.InsertAsync(new Database.SqlServer.Models.User
            {
                FirstName = addUserRequest.FirstName,
                LastName = addUserRequest.LastName,
                Email = addUserRequest.Email,
                PasswordHash = addUserRequest.Password,
                BalanceFunds = addUserRequest.BalanceFunds,
                RiskCoefficient = addUserRequest.RiskCoefficient,
                DateCreated = DateTime.Now,
                TacticsId = addUserRequest.TackticId
            });

            if (newUserId == null)
            {
                Log.Warning("Failed creating user, user with {Email} already exists", addUserRequest.Email);
                return UnprocessableEntity(new ApiResult(ErrorCodes.AlreadyExists, "User already exists"));
            }

            Log.Information($"Successfully created user {addUserRequest.FirstName} {addUserRequest.LastName}");

            return Created("", new ApiResult<BaseResponse>(new BaseResponse
            {
                Message = "User successfully created",
                Id = (int)newUserId
            }));
        }

        [HttpPut]
        [Route("EditUser")]
        public async Task<IActionResult> EditUser([FromBody] UpdateUserRequest updateUserRequest)
        {
            if (string.IsNullOrEmpty(updateUserRequest.FirstName))
            {
                Log.Warning("Failed creating user, no user first name");
                return BadRequest(new ApiResult(ErrorCodes.MissingParameters, "Empty user FirstName"));
            }

            if (string.IsNullOrEmpty(updateUserRequest.LastName))
            {
                Log.Warning("Failed creating user, no user last name");
                return BadRequest(new ApiResult(ErrorCodes.MissingParameters, "Empty user LastName"));
            }

            if (string.IsNullOrEmpty(updateUserRequest.Email))
            {
                Log.Warning("Failed creating user, no user email");
                return BadRequest(new ApiResult(ErrorCodes.MissingParameters, "Empty user Email"));
            }

            var updatedUser = await _userService.UpdateAsync(new StockMarket.Database.SqlServer.Models.User
            {
                Id = updateUserRequest.Id,
                FirstName = updateUserRequest.FirstName,
                LastName = updateUserRequest.LastName,
                Email = updateUserRequest.Email,
                PasswordHash = updateUserRequest.Password,
                BalanceFunds = updateUserRequest.BalanceFunds,
                RiskCoefficient = updateUserRequest.RiskCoefficient,
                DateUpdated = DateTime.Now,
                TacticsId = updateUserRequest.TackticId
            });

            if (updatedUser == null)
                return UnprocessableEntity(new ApiResult(ErrorCodes.NotFound, $"No user with Id: {updateUserRequest.Id} has been found"));

            Log.Information("User with Id: {Id} has been successfully edited", updateUserRequest.Id);
            return Ok(new ApiResult<BaseResponse>(new BaseResponse { Message = $"User has been edited", Id = updateUserRequest.Id }));
        }   
    }
}
