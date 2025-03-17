using LoginService.App.Models;
using LoginService.App.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginService.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public LoginController(UserManager<ApplicationUser> userManager, ITokenService tokenService, IEmailService emailService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user == null)
            {
                return NotFound(new { Message = "Usuário não encontrado." });
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, login.Password);
            if (!isPasswordValid)
            {
                return Unauthorized(new { Message = "Senha inválidos." });
            }

            var token = await _tokenService.GenerateToken(login);
            return Ok(new { Token = token });
        }

        [HttpGet("getUsers")]
        public async Task<ActionResult<List<ApplicationUser>>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            if (users == null || users.Count == 0)
            {
                return NotFound(new { Message = "Nenhum usuário cadastrado" });
            }

            return Ok(users);
        }

        [HttpGet("getUser")]
        public async Task<IActionResult> GetUser([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound(new { Message = "Usuário não encontrado." });
            }

            return Ok(user);
        }

        [HttpPost("createUser")]
        public async Task<ActionResult<ApplicationUser>> CreateUser([FromBody] CreateUser createUser)
        {
            var user = new ApplicationUser
            {
                UserName = createUser.Email,
                Email = createUser.Email,
                Role = createUser.Role
            };

            var result = await _userManager.CreateAsync(user, createUser.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return CreatedAtAction(nameof(GetUsers), new { email = user.Email }, user);
        }

        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser([FromQuery] string email, [FromBody] UpdateUser updateUser)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound(new { Message = "Usuário não encontrado." });
            }

            user.Role = updateUser.Role;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(user);
        }

        [HttpDelete("deleteUser")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound(new { Message = "Usuário não encontrado." });
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok($"{email} deletado com sucesso.");
        }

        [HttpPost("forgotPasswordUser")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword forgotPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);

            if (user == null)
            {
                return NotFound(new { Message = "Usuário não encontrado." });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Login", new { token, email = forgotPassword.Email }, Request.Scheme);

            await _emailService.SendEmailAsync(
                user, "Redefinição de Senha", $"Clique aqui para redefinir sua senha: <a href='{resetLink}'> Redefinir Senha</a>");

            return Ok("Link de redefuinição de senha enviado");
        }

        [HttpPost("resetPasswordUser")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);

            if (user == null)
            {
                return NotFound(new { Message = "Usuário não encontrado." });
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Senha redefinida com sucesso.");
        }
    }
}
