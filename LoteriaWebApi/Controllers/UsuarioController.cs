using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Entidades.Models;
using LogicaNegocio.Implementacion;
using LogicaNegocio.Interfaz;
using MetodosComunes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// Alias para el ILogger de Serilog
using SerilogLogger = Serilog.ILogger;

// Alias para el ILogger de Microsoft.Extensions.Logging

namespace LoteriaWebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

    
        //private readonly ILogger<UsuarioController> _logger;

        public IConfiguration lConfiguration;

        private readonly IUsuarioLN gObjUsuarioLN;

        public Excepciones gObjExcepciones = new Excepciones();

        private readonly SerilogLogger _logger = Log.ForContext<UsuarioController>();



        public UsuarioController(IConfiguration lConfig)
        {
            lConfiguration = lConfig;
            string? lCadenaConexcion = lConfiguration.GetConnectionString("LoteriaBD");

            gObjUsuarioLN = new UsuarioLN(lConfiguration);
        }

        private string GenerarJwtToken(Usuario usuario)
        {
            _logger.Information($"Generando token JWT para el usuario: {usuario.NombreUsuario}");

            var keyVaultUrl = lConfiguration["AzureKeyVault:VaultUrl"];
            var client = new SecretClient(new Uri(keyVaultUrl ?? ""), new DefaultAzureCredential());
            var secret = client.GetSecret("KeyVaultLoteriaLO").Value.Value;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var issuer = lConfiguration["JwtI:Issuer"] ?? "";
            var audience = lConfiguration["JwtA:Audience"] ?? "";

            // --- Header ---
            var header = new JwtHeader(credentials);

            // --- Payload ---
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim("Id", usuario.Id.ToString()),
                new Claim("Rol", usuario.Rol.ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, issuer ),
                new Claim(JwtRegisteredClaimNames.Aud, audience),
                new Claim("Correo", usuario.Correo)
            };

            var payload = new JwtPayload(
                issuer,               // Issuer
                audience,             // Audience
                claims,               // Claims
                DateTime.UtcNow,      // NotBefore (Inicio de validez)
                DateTime.UtcNow.AddHours(1) // Expiration (Expiración en 1 hora)
            );

            // --- Combine Header and Payload ---
            var token = new JwtSecurityToken(header, payload);

            _logger.Information("Token JWT generado exitosamente");

            _logger.Information(token.ToString());
            _logger.Information("Token JWT generado exitosamente");
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Manejo de errores
        private ActionResult ManejoError(Exception ex)
        {
            _logger.Error($"Error ocurrido: {ex.Message}");
            if (ex.InnerException != null)
            {
                _logger.Warning($"Causa del error: {ex.InnerException.Message}");
            }

            gObjExcepciones.LogError(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

        // Manejo de respuestas
        private IActionResult HandleResponse<T>(T response)
        {
            if (response == null)
            {
                _logger.Warning("Respuesta no encontrada, retornando 404");
                return NotFound();
            }

            _logger.Information("Respuesta exitosa, retornando 200 OK");
            return Ok(response);
        }

        // Endpoint de login
        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public IActionResult Login([FromBody] Usuario usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.Clave))
            {
                _logger.Warning("Credenciales inválidas proporcionadas");
                return BadRequest("Credenciales inválidas");
            }

            try { 
            
                _logger.Information($"Intentando loguear al usuario: {usuario.NombreUsuario}");
                var user = gObjUsuarioLN.ValidarLoginUsuario(usuario.Id, usuario.Clave);
                if (user == null)
                {
                    _logger.Warning("Usuario o Clave Incorrecta");
                    return Unauthorized("Usuario o Clave Incorrecta");
                }

                var token = GenerarJwtToken(user);
                _logger.Information($"Usuario {user.NombreUsuario} autenticado exitosamente");

                return Ok(new { Token = token, user });
            }
            catch (Exception ex)
            {
                return ManejoError(ex);
            }
        }

        // Endpoint para obtener los usuarios (restringido)
        [HttpGet("RecUsuario")]
        public ActionResult<List<Usuario>> RecUsuario()
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                _logger.Information("Recuperando la lista de usuarios");
                usuarios = gObjUsuarioLN.RecUsuario();

                _logger.Information($"Se recuperaron {usuarios.Count} usuarios");
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return ManejoError(ex);
            }
        }

        // Endpoint para obtener usuario por ID (restringido)
        [Route("RecUsuarioXId")]
        [HttpPost]
        public IActionResult RecUsuarioXId([FromBody] Usuario usuario)
        {
            try
            {
                _logger.Information($"Recuperando usuario con ID: {usuario.Id}");
                var user = gObjUsuarioLN.RecUsuarioXId(usuario.Id);
                return HandleResponse(user);
            }
            catch (Exception ex)
            {
                return ManejoError(ex);
            }
        }

        // Endpoint para insertar usuario (restringido)
        [Authorize]
        [Route("InsUsuario")]
        [HttpPost]
        public IActionResult InsUsuario([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                _logger.Warning("Modelo de usuario inválido");
                return BadRequest("Modelo Inválido");
            }

            try
            {
                _logger.Information($"Insertando nuevo usuario: {usuario.NombreUsuario}");
                gObjUsuarioLN.InsUsuario(usuario);
                _logger.Information($"Usuario {usuario.NombreUsuario} insertado exitosamente");
                return CreatedAtAction(nameof(RecUsuarioXId), new { id = usuario.Id }, usuario);
            }
            catch (Exception ex)
            {
                return ManejoError(ex);
            }
        }

        // Endpoint para eliminar usuario (restringido)
        [Authorize]
        [Route("DelUsuario")]
        [HttpDelete]
        public IActionResult DelUsuario([FromBody] int idUsuario)
        {
            try
            {
                _logger.Information($"Eliminando usuario con ID: {idUsuario}");
                var usuario = gObjUsuarioLN.RecUsuarioXId(idUsuario);
                if (usuario == null)
                {
                    _logger.Warning($"Usuario con ID {idUsuario} no encontrado");
                    return BadRequest("Usuario No Encontrado");
                }

                gObjUsuarioLN.DelUsuario(usuario);
                _logger.Information($"Usuario con ID {idUsuario} eliminado exitosamente");
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return ManejoError(ex);
            }
        }

        // Logout del usuario
        [Route("Logout")]
        [HttpPost]
        public IActionResult Logout()
        {
            _logger.Information("Logout de usuario realizado");
            Response.Cookies.Append("Token", "", new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            });

            return Ok(new { message = "Usuario deslogueado exitosamente" });
        }

        // Eliminar cookie de sesión
        [Route("EliminaCookie")]
        [HttpPost]
        public IActionResult EliminaCookie()
        {
            _logger.Information("Eliminando cookie de sesión");
            Response.Cookies.Delete("Token");

            return Ok(new { message = "Cookie eliminada exitosamente" });
        }

    }
}

