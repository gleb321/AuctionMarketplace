using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AuthenticationServer.Models;
using AuthenticationServer.Services;
using AuctionMarketplaceLibrary;

namespace AuthenticationServer.Controllers {
    [ApiController]
    [Route("/account/")]
    public class AccountController: Controller {
        private readonly IDbContextFactory<PgUsersDataBaseContext> _pgDataBaseFactory;
        
        public AccountController(IDbContextFactory<PgUsersDataBaseContext> pgDataBaseFactory) {
            _pgDataBaseFactory = pgDataBaseFactory;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel registrationAccount) {
            User user = new User(registrationAccount.Email, registrationAccount.Name, registrationAccount.Surname);
            Account account = new Account(registrationAccount.Email,
                Cryptographer.HashPassword(registrationAccount.Password), Account.Role.User);

            var pgDataBaseContexts = await Task.WhenAll(
                _pgDataBaseFactory.CreateDbContextAsync(), _pgDataBaseFactory.CreateDbContextAsync());

            await Task.WhenAll(
                pgDataBaseContexts[0].Users.AddAsync(user).AsTask(), pgDataBaseContexts[1].Accounts.AddAsync(account).AsTask());

            try {
                await Task.WhenAll(
                    pgDataBaseContexts[0].SaveChangesAsync(), pgDataBaseContexts[1].SaveChangesAsync());
            } catch (DbUpdateException exception) {
                //TODO Детальнее проработать exception
                return BadRequest("User with such a login already exists.");
            }

            return Ok("Successful registration.");
        }
        
        [HttpPost("auth")]
        public async Task<IActionResult> Authorise([FromBody] AuthenticationModel authenticationAccount) {
            Account? databaseAccount;
            using (var pgDataBase = await _pgDataBaseFactory.CreateDbContextAsync()) {
                databaseAccount = await (from account in pgDataBase.Accounts
                    where account.Login == authenticationAccount.Login
                    select account).SingleOrDefaultAsync();
            }

            if (databaseAccount is null) {
                return NotFound("User with this login has not been registered yet");
            }

            if (databaseAccount.Password != Cryptographer.HashPassword(authenticationAccount.Password)) {
                return BadRequest("Incorrect login or password.");
            }

            string accessToken = Authenticator.CreateToken(Authenticator.AccessTokenLifetime,
                Authenticator.TokenType.Access, databaseAccount.Login, databaseAccount.UserRole.ToString());
            string refreshToken = Authenticator.CreateToken(Authenticator.RefreshTokenLifetime,
                Authenticator.TokenType.Refresh, databaseAccount.Login, databaseAccount.UserRole.ToString());
            
            return Ok(new {
                 AccessToken = accessToken,
                 RefreshToken = refreshToken
            });
        }
        
        [Authorize]
        [HttpGet("refresh")]
        public IActionResult Refresh() {
            string refreshToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            (string login, string role) = JwtParser.GetClaims(refreshToken, Authenticator.TokenType.Refresh);
            double minutesToExpiration = JwtParser.GetMinutesBeforeExpiration(refreshToken, Authenticator.TokenType.Refresh);
            
            string accessToken = Authenticator.CreateToken(Authenticator.AccessTokenLifetime, Authenticator.TokenType.Access, login, role);
            if (minutesToExpiration < 15) {
                refreshToken = Authenticator.CreateToken(Authenticator.RefreshTokenLifetime, Authenticator.TokenType.Refresh, login, role);
            }
            
            return Ok(new {
                 AccessToken = accessToken,
                 RefreshToken = refreshToken
            });
        }
    }
}