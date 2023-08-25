using StockMarket.Database.SqlServer.Models;
using System.Text.Json.Serialization;

namespace StockMarket.Server.Models.Responses
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JwtToken { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }

        public AuthenticateResponse(User user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
