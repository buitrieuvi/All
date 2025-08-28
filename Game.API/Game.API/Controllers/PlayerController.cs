using Game.API.Model;
using Game.API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;

namespace Game.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly PlayerService _playerService;
        private readonly UserService _userService;

        private readonly JwtSettings _jwtSettings;

        private readonly HttpContext _httpContext;

        public PlayerController(
            UserService userService,
            PlayerService playerService,
            IOptions<JwtSettings> jwtSettings)
        {
            _playerService = playerService;
            _playerService = playerService;

            _jwtSettings = jwtSettings.Value;
        }

        [HttpGet("getplayer")]
        [Authorize]
        public async Task<IActionResult> GetPlayer() 
        {
            //if (!CheckToken())
            //    return Unauthorized(new APIResponse<string>("401", "Phiên đăng nhập đã hết hạn", ""));

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            Player player = await _playerService.GetPlayerById(userId);

            if (player == null) 
            {
                return NotFound(userId);
            }

            return Ok(new APIResponse<Player>("200", "", player));
        }

        [HttpGet("replaceplayer")]
        [Authorize]
        public async Task<IActionResult> ReplacePlayer([FromBody] Player newPlayer)
        {
            //if (!CheckToken())
            //    return Unauthorized(new APIResponse<string>("401", "Phiên đăng nhập đã hết hạn", ""));

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            Player player = await _playerService.GetPlayerById(userId);

            if (player == null)
            {
                return NotFound();
            }
            newPlayer.Id = player.Id;

            await _playerService.ReplacePlayer(newPlayer);
            return Ok();
        }

        private bool CheckToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var exp = jwtToken.ValidTo;
            
            if (DateTime.UtcNow.AddHours(7) > exp)
            {
                return false;
            }

            return true;
        }
    }
}
