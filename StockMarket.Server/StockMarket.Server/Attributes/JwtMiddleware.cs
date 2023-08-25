using StockMarket.Server.Services.Interfaces;

namespace StockMarket.Server.Attributes
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var userId = jwtUtils.ValidateJwtToken(token);
                if (userId != null)
                {
                    context.Items["User"] = userService.GetAsync(userId.Value);
                }
            }

            await _next(context);
        }
    }
}
