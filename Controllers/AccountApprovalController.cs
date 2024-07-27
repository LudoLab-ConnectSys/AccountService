using AccountService.Data;
using AccountService.Models;

using Microsoft.AspNetCore.Mvc;
using AccountService.Services;

namespace AccountService.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountApprovalController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _context;

        public AccountApprovalController(IEmailService emailService, ApplicationDbContext context)
        {
            _emailService = emailService;
            _context = context;
        }

        [HttpPost("approve")]
        public async Task<IActionResult> ApproveAccount([FromBody] AccountRequest request)
        {
            var user = await _context.Usuario.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Generar y encriptar la contraseña temporal
            string temporaryPassword = $"Lc@{user.Cedula}";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(temporaryPassword);
            user.Password = hashedPassword;

            _context.Usuario.Update(user);
            await _context.SaveChangesAsync();

            string subject = "Cuenta Aprobada";
            string message = $"Hola {user.NombreUsuario} {user.ApellidoUsuario},\n\n" +
                             $"Su cuenta ha sido aprobada. Sus credenciales son:\n" +
                             $"Usuario: {user.Email}\nContraseña temporal: {temporaryPassword}";

            await _emailService.SendEmailAsync("ginno.taimal@gmail.com", subject, message);

            return Ok();
        }

        [HttpPost("deny")]
        public async Task<IActionResult> DenyAccount([FromBody] AccountRequest request)
        {
            var user = await _context.Usuario.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            string subject = "Cuenta No Aprobada";
            string message = $"Hola {user.NombreUsuario} {user.ApellidoUsuario},\n\n" +
                             "Lamentamos informarle que su solicitud de cuenta no ha sido aprobada.";

            await _emailService.SendEmailAsync("ginno.taimal@gmail.com", subject, message);

            return Ok();
        }
    }
}