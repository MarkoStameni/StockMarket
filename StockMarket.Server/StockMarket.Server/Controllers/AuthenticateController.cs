using StockMarket.Server.Models;
using StockMarket.Server.Models.Responses;
using StockMarket.Server.Requests;
using StockMarket.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StockMarket.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticateController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser(AuthenticateRequest model)
        {
            if (model.Email != "" && model.Password != "")
            {
                var response = await _userService.Authenticate(model, IpAddress());
                if (response != null)
                {
                    SetTokenCookie(response.RefreshToken);
                    return Created("", new ApiResult<AuthenticateResponse>(response));
                }

                return BadRequest(new ApiResult(ErrorCodes.InvalidParameters, "Incorect password or email"));
            }
            else
                return BadRequest(new ApiResult(ErrorCodes.InvalidParameters, "Password or email is empty"));
            
        }

        [AuthorizeAttribute]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken != null)
            {
                var response = await _userService.RefreshToken(refreshToken, IpAddress());
                if (response != null)
                {
                    SetTokenCookie(response.RefreshToken);
                    return Ok(response);
                }

                return BadRequest(new ApiResult(ErrorCodes.InvalidParameters, "Incorect password or email"));
            }

            return BadRequest(new ApiResult(ErrorCodes.InvalidParameters, "Refresh token from cookie is null"));
        }

        private void SetTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string IpAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
