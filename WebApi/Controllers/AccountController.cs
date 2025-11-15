using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Super_Cartes_Infinies.Models;
using Super_Cartes_Infinies.Services;
using Models.Models.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using static System.Net.WebRequestMethods;
using System.Data;
using Microsoft.Identity.Client;
using Super_Cartes_Infinies.Data;
using Microsoft.EntityFrameworkCore;

namespace Super_Cartes_Infinies.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly PlayersService _playersService;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, PlayersService playersService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _playersService = playersService;
            _context = context;
        }

        /// <summary>
        /// Inscription d'un nouvel utilisateur.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Register(RegisterDTO registerDTO)
        {
            if (registerDTO.Password != registerDTO.PasswordConfirm)
            {
                return BadRequest(new { Error = "Le mot de passe et la confirmation ne sont pas identiques" });
            }

            IdentityUser user = new IdentityUser()
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email
            };

            IdentityResult identityResult = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!identityResult.Succeeded)
            {
                return BadRequest(new { Error = identityResult.Errors });
            }

            GameConfig gameConfig = await _context.GameConfigs.FirstOrDefaultAsync();
            if (gameConfig == null)
            {
                return StatusCode(500, new { Error = "Configuration de jeu introuvable" });
            }

            _playersService.CreatePlayer(user, gameConfig);

            return Ok(new { Message = "Utilisateur enregistré avec succès" });
        }

        /// <summary>
        /// Connexion d'un utilisateur existant.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Login(LoginDTO loginDTO)
        {
            var result = await _signInManager.PasswordSignInAsync(loginDTO.Username, loginDTO.Password, true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(loginDTO.Username);
                if (user == null)
                {
                    return NotFound(new { Error = "User not found." });
                }

                var player = _playersService.GetPlayerFromUserId(user.Id);
                if(player == null)
                {
                    return NotFound(new { Error = "Player not found." });
                }

                SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("C'est tellement la meilleure cle qui a jamais ete cree dans l'histoire de l'humanite (doit etre longue)"));
                IList<string> roles = await _userManager.GetRolesAsync(user);
                List<Claim> authClaims = new List<Claim>();
                foreach (string role in roles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }
                authClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                string issuer = "https://localhost:7179";

                DateTime expirationTime = DateTime.Now.AddMinutes(30);

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: "http://localhost:4200",
                    claims: authClaims,
                    expires: expirationTime,
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
                );

                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new LoginSuccessDTO()
                {
                    Token = tokenString,
                    UserId = user.Id,
                    Username = user.UserName,
                    PlayerMoney = player.Money
                });
            }

            return NotFound(new { Error = "L'utilisateur est introuvable ou le mot de passe ne concorde pas" });
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Player>> GetCurrentPlayer()
        {
            // Récupérer l'utilisateur connecté à partir du contexte
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Error = "Utilisateur non authentifié." });
            }

            // Récupérer le joueur associé à l'utilisateur
            var player = _playersService.GetPlayerFromUserId(userId);
            if (player == null)
            {
                return NotFound(new { Error = "Joueur introuvable." });
            }

            return Ok(player);
        }

        [Authorize]
        [HttpGet]
        public ActionResult<string[]> PrivateData()
        {
            return Ok( new string[] { "figue", "banane", "noix" });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<int>> GetPlayerELO()
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (user != null)
                return _playersService.GetPlayerFromUserId(user.Id).ELO;

            return -1;
        }
    }
}
