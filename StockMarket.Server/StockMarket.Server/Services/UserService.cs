using AutoMapper;
using StockMarket.Database.SqlServer;
using StockMarket.Database.SqlServer.Models;
using StockMarket.Server.Helpers;
using StockMarket.Server.Models.Responses;
using StockMarket.Server.Requests;
using StockMarket.Server.Services.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using Serilog;

namespace StockMarket.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;
        private IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;

        public UserService(
       IDbContextFactory<DatabaseContext> dbContextFactory,
            IJwtUtils jwtUtils,
            IOptions<AppSettings> appSettings,
            IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _jwtUtils = jwtUtils;
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        public async Task<int?> InsertAsync(User user)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var exists = await dbContext.Users.SingleOrDefaultAsync(x => x.Email == user.Email);
            if (exists != null)
            {
                Log.Warning("Failed inserting user, user already exists with email: {Email}", user.Email);
                return null;
            }

            // Generisanje "salt" vrijednosti
            byte[] salt = GenerateSalt();

            // Kreiranje hash-a lozinke uz pomoć PBKDF2 algoritma
            var hashedPassword = GetHashedPassword(user.PasswordHash, salt);

            // Konverzija salt-a u string format za upisivanje u bazu podataka
            var saltString = Convert.ToBase64String(salt);
            user.PasswordHash = hashedPassword;
            user.Salt = saltString;
            user.TacticsId = user.TacticsId;
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            Log.Information($"User created with name {user.FirstName} {user.LastName}");

            return user.Id;
        }

        public static string GetHashedPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                   password: password,
                   salt: salt,
                   prf: KeyDerivationPrf.HMACSHA256,
                   iterationCount: 100000,
                   numBytesRequested: 256 / 8));
        }

        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[32]; // 32 byte-a je preporučena dužina salt-a
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public async Task<User?> UpdateAsync(User user)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var entity = await dbContext.Users.SingleOrDefaultAsync(x => x.Id == user.Id);
            if (entity == null)
            {
                Log.Warning("Failed updating user, no user with id: {Id}", user.Id);
                return null;
            }

            entity.FirstName = user.FirstName;
            entity.LastName = user.LastName;
            entity.Email = user.Email;
            entity.BalanceFunds = user.BalanceFunds;
            entity.RiskCoefficient = user.RiskCoefficient;

            // Update properties of the related Tactics entity
            if (user.Tactics != null)
            {
                entity.TacticsId = user.TacticsId;
            }

            dbContext.Update(entity);
            await dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest authetificationRequest, string ipAddress)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var user = await dbContext.Users.Include(x => x.RefreshTokens).SingleOrDefaultAsync(x => x.Email == authetificationRequest.Email);
            if (user != null)
            {
                // validate
                var hashedPassword = ComputeHash(authetificationRequest.Password, user.Salt);
                if (hashedPassword != user.PasswordHash)
                {
                    Log.Warning("Failed logging in user, incorrect password for email: {Email}", authetificationRequest.Email);
                    return null;
                }

                // authentication successful so generate jwt and refresh tokens
                var jwtToken = _jwtUtils.GenerateJwtToken(user);
                var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);

                if (refreshToken != null)
                    user.RefreshTokens.Add(refreshToken);

                // remove old refresh tokens from user
                if (user.RefreshTokens.Count() > 0)
                    RemoveOldRefreshTokens(user);

                // save changes to db
                dbContext.Update(user);
                dbContext.SaveChanges();

                return new AuthenticateResponse(user, jwtToken, refreshToken!.Token);
            }

            return null;
        }

        public async Task<AuthenticateResponse?> RefreshToken(string token, string ipAddress)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var user = await dbContext.Users.Include(x => x.RefreshTokens).SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user != null)
            {
                var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
                if (refreshToken.IsRevoked)
                {
                    // revoke all descendant tokens in case this token has been compromised
                    RevokeDescendantRefreshTokens(refreshToken, user, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
                    dbContext.Update(user);
                    dbContext.SaveChanges();
                }

                if (!refreshToken.IsActive)
                {
                    Log.Warning("Invalid token");
                    return null;
                }

                // replace old refresh token with a new one (rotate token)
                var newRefreshToken = RotateRefreshToken(refreshToken, ipAddress);
                if (newRefreshToken != null)
                    user.RefreshTokens.Add(newRefreshToken);

                // remove old refresh tokens from user
                if (user.RefreshTokens.Count() > 0)
                    RemoveOldRefreshTokens(user);

                // save changes to db
                dbContext.Update(user);
                dbContext.SaveChanges();

                // generate new jwt
                var jwtToken = _jwtUtils.GenerateJwtToken(user);

                return new AuthenticateResponse(user, jwtToken, newRefreshToken!.Token);
            }

            return null;
        }

        public async Task<List<UserResponse>> GetListAsync()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var users = await dbContext.Users.ToListAsync();

            var response = _mapper.Map<List<UserResponse>>(users);
            return response;
        }

        public async Task<UserResponse?> GetAsync(int id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var user = await dbContext.Users
                .Include(x => x.Tactics)
                .Include(x => x.CompanyUsers)
                .ThenInclude(x => x.Company)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return null;

            var response = _mapper.Map<UserResponse>(user);
            return response;
        }

        private RefreshToken RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
        {
            var newRefreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
            RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }

        private void RemoveOldRefreshTokens(User user)
        {
            // remove old inactive refresh tokens from user based on TTL in app settings
            user.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        private void RevokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason)
        {
            // recursively traverse the refresh token chain and ensure all descendants are revoked
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
                if (childToken!.IsActive)
                    RevokeRefreshToken(childToken, ipAddress, reason);
                else
                    RevokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
            }
        }

        private void RevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }

        private static string ComputeHash(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            return ComputeHash(password, saltBytes);
        }

        private static string ComputeHash(string password, byte[] salt)
        {
            return Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                   password: password,
                   salt: salt,
                   prf: KeyDerivationPrf.HMACSHA256,
                   iterationCount: 100000,
                   numBytesRequested: 256 / 8));
        }
    }
}
