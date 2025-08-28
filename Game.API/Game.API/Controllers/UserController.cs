using Game.API.Model;
using Game.API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Game.API.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly PlayerService _playerService;

        private readonly JwtSettings _jwtSettings;

        public UserController(
            UserService userService,
            PlayerService playerService,
            IOptions<JwtSettings> jwtSettings)
        {
            _userService = userService;
            _playerService = playerService;

            _jwtSettings = jwtSettings.Value;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User.Register user)
        {
            User existingUser = await _userService.GetUserByUsername(user.UserName);
            if (existingUser != null)
            {
                return Conflict(new APIResponse<string>("409", "Tài khoản đã tồn tại", ""));
            }

            string pwHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

            User newUser = new User(ObjectId.GenerateNewId().ToString(), user.UserName, pwHash);

            await _userService.CreateUser(newUser);
            await _playerService.CreatePlayer(newUser);
            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User.Login user)
        {
            User existingUser = await _userService.GetUserByUsername(user.UserName);

            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.PasswordHash))
            {
                return Unauthorized(new APIResponse<string>("401", "Thông tin đăng nhập không đúng", ""));
            }
            string accessToken = GenerateJwtToken(existingUser);
            string refreshToken = GenerateRefreshToken();

            existingUser.RefreshToken = refreshToken;
            existingUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userService.UpdateUser(existingUser);

            return Ok(new APIResponse<User.Token>("200", "", new User.Token(accessToken, refreshToken)));

        }

        [Authorize]
        [HttpGet("getname")]
        public async Task<IActionResult> GetName()
        {
            //if (!CheckToken())
            //    return Unauthorized(new APIResponse<string>("401", "Phiên đăng nhập đã hết hạn", ""));

            var userName = User.FindFirst(ClaimTypes.Name).Value;

            var user = await _userService.GetUserByUsername(userName);
            if (user == null)
                return NotFound(new APIResponse<string>("404", "không tìm thấy", ""));

            return Ok(new APIResponse<string>("200", "", user.UserName));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] User.Token tokenModel)
        {
            var principal = GetPrincipalFromExpiredToken(tokenModel.AccessToken);
            if (principal == null)
                return BadRequest(new APIResponse<string>("400", "Token không hợp lệ", ""));

            string username = principal.Identity.Name;
            var user = await _userService.GetUserByUsername(username);

            if (user == null || user.RefreshToken != tokenModel.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized(new APIResponse<string>("401", "Refresh token không hợp lệ hoặc đã hết hạn", ""));
            }

            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(7).AddDays(7);
            await _userService.UpdateUser(user);

            return Ok(new APIResponse<User.Token>("200", "", new User.Token(newAccessToken, newRefreshToken)));
        }

        private bool CheckToken()
        {
            if (User.Identity.IsAuthenticated) 
            {
                return true;
            }
            return false;

            //var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            //var handler = new JwtSecurityTokenHandler();
            //var jwtToken = handler.ReadJwtToken(token);

            //var exp = jwtToken.ValidTo;

            //if (DateTime.UtcNow.AddHours(7) > exp)
            //{
            //    return false;
            //}

            //return true;
        }
        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(7).AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }
    }
}
