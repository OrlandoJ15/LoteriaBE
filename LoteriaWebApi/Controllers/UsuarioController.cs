using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Entidades.Models;
using LogicaNegocio.Implementacion;
using LogicaNegocio.Interfaz;
using MetodosComunes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Serilog;
using Microsoft.Extensions.Logging;  // No hace falta importar esto explícitamente, si lo tienes.

// Alias para el ILogger de Serilog
using SerilogLogger = Serilog.ILogger;

// Alias para el ILogger de Microsoft.Extensions.Logging
using MicrosoftLogger = Microsoft.Extensions.Logging.ILogger;

namespace LoteriaWebApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IConfiguration lConfiguration;

        private readonly IUsuarioLN gObjUsuarioLN;

        private readonly Excepciones gObjExcepciones = new Excepciones();

        private readonly SerilogLogger _logger;


        public UsuarioController(IConfiguration lConfig, ILogger<UsuarioController> logger)
        {
            lConfiguration = lConfig;
            string? lCadenaConexcion = lConfiguration.GetConnectionString("LoteriaBD");

            gObjUsuarioLN = new UsuarioLN(lConfiguration);
            _logger = Log.ForContext<UsuarioController>();  // Inicializar el logger de Serilog // Asignamos el logger
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.Information("GET request to /Usuario endpoint");

            return Ok(new { message = "Hello" });
        }

        private string GenearJwtToken(Usuario pUsuario)
        {
            _logger.Information($"Generando token JWT para el usuario: {pUsuario.NombreUsuario}");

            var keyVaultUrl = lConfiguration["AzureKeyVault:VaultUrl"];

            var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
            var secret = client.GetSecret("JwtKey").Value.Value;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, pUsuario.NombreUsuario),
                new Claim("Id", pUsuario.Id.ToString()),
                new Claim("Rol", pUsuario.Rol.ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, lConfiguration["Jwt:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Aud, lConfiguration["Jwt:Audience"]),
                new Claim("Correo", pUsuario.Correo),
                new Claim("FechaCreacion", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            };

            var token = new JwtSecurityToken(
                issuer: lConfiguration["Jwt:Issuer"],
                audience: lConfiguration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: credentials
            );

            _logger.Information("Token JWT generado exitosamente");

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private ActionResult ManejoError(Exception Ex)
        {
            _logger.Error($"Error ocurrido: {Ex.Message}");
            gObjExcepciones.LogError(Ex);
            return StatusCode(StatusCodes.Status500InternalServerError, Ex.Message);
        }

        private IActionResult HandleResponse<T>(T response)
        {
            if (response == null)
            {
                _logger.Warning("Respuesta no encontrada, retornando 404");
                return new JsonResult(null);
            }

            _logger.Information("Respuesta exitosa, retornando 200 OK");
            return Ok(response);
        }

        [HttpGet]
        public IActionResult Getu()
        {
            _logger.Information("GET request to /Usuario/Getu endpoint");
            return Ok(new { message = "Hello" });
        }

        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public IActionResult Login([FromBody] Usuario pUsuario)
        {
            if (pUsuario == null || string.IsNullOrEmpty(pUsuario.Clave))
            {
                _logger.Warning("Credenciales inválidas proporcionadas");
                return BadRequest("Credenciales Invalidas");
            }

            try
            {
                _logger.Information($"Intentando loguear al usuario: {pUsuario.NombreUsuario}");
                var user = gObjUsuarioLN.ValidarLoginUsuario(pUsuario.Id, pUsuario.Clave);
                if (user == null)
                {
                    _logger.Warning("Usuario o Clave Incorrecta");
                    return Unauthorized("Usuario o Clave Incorrecta");
                }

                var token = GenearJwtToken(user);
                _logger.Information($"Usuario {user.NombreUsuario} autenticado exitosamente");

                return Ok(new { Token = token, user });
            }
            catch (Exception ex)
            {
                return ManejoError(ex);
            }
        }

        [Authorize]
        [Route("RecUsuario")]
        [HttpGet]
        public ActionResult<List<Usuario>> RecUsuario()
        {
            List<Usuario> lObjRespuesta = new List<Usuario>();

            try
            {
                _logger.Information("Recuperando la lista de usuarios");
                lObjRespuesta = gObjUsuarioLN.RecUsuario();

                var Usuario = lObjRespuesta.Select(u => new Usuario
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    NombreUsuario = u.NombreUsuario,
                    Rol = u.Rol,
                    Correo = u.Correo
                }).ToList();

                _logger.Information($"Se recuperaron {Usuario.Count} usuarios");
                return Ok(Usuario);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        [Authorize]
        [Route("[action]")]
        [HttpPost]
        public IActionResult? RecUsuarioXId([FromBody] Usuario pUsuario)
        {
            try
            {
                _logger.Information($"Recuperando usuario con ID: {pUsuario.Id}");
                var lObjRespuesta = gObjUsuarioLN.RecUsuarioXId(pUsuario.Id);
                return HandleResponse(lObjRespuesta);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        [Authorize]
        [Route("[action]")]
        [HttpPost]
        public IActionResult InsUsuario([FromBody] Usuario pUsuario)
        {
            if (!ModelState.IsValid)
            {
                _logger.Warning("Modelo de usuario inválido");
                return BadRequest("Modelo Invalido");
            }

            try
            {
                _logger.Information($"Insertando nuevo usuario: {pUsuario.NombreUsuario}");
                gObjUsuarioLN.InsUsuario(pUsuario);
                _logger.Information($"Usuario {pUsuario.NombreUsuario} insertado exitosamente");
                return CreatedAtAction(nameof(RecUsuarioXId), new { pIdUsuario = pUsuario.Id }, pUsuario);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        [Authorize]
        [Route("[action]")]
        [HttpPut]
        public IActionResult ModUsuario([FromBody] Usuario pUsuario)
        {
            if (!ModelState.IsValid)
            {
                _logger.Warning("Modelo de usuario inválido para modificación");
                return BadRequest("Modelo Invalido");
            }

            try
            {
                _logger.Information($"Modificando usuario con ID: {pUsuario.Id}");
                gObjUsuarioLN.ModUsuario(pUsuario);
                _logger.Information($"Usuario {pUsuario.Id} modificado exitosamente");
                return Ok(pUsuario);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        [Authorize]
        [Route("[action]")]
        [HttpDelete]
        public IActionResult DelUsuario([FromBody] int IdUsuario)
        {
            try
            {
                _logger.Information($"Eliminando usuario con ID: {IdUsuario}");
                var lUsuario = gObjUsuarioLN.RecUsuarioXId(IdUsuario);
                if (lUsuario == null)
                {
                    _logger.Warning($"Usuario con ID {IdUsuario} no encontrado");
                    return BadRequest("Usuario No Encontrado");
                }
                gObjUsuarioLN.DelUsuario(lUsuario);
                _logger.Information($"Usuario con ID {IdUsuario} eliminado exitosamente");
                return Ok(lUsuario);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult? ValidarUsuarioLogin([FromBody] Usuario pUsuario)
        {
            if (!ModelState.IsValid || pUsuario == null || string.IsNullOrEmpty(pUsuario.Clave))
            {
                _logger.Warning("Datos inválidos o incompletos para validación de login");
                return BadRequest("Datos inválidos o incompletos");
            }

            try
            {
                _logger.Information($"Validando login para el usuario con ID: {pUsuario.Id}");
                var lUsuario = gObjUsuarioLN.ValidarLoginUsuario(pUsuario.Id, pUsuario.Clave);
                return HandleResponse(lUsuario);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        [Authorize]
        [Route("[action]")]
        [HttpPut]
        public IActionResult ModClaveUsuario([FromBody] Usuario pUsuario)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(pUsuario.Clave))
            {
                _logger.Warning("Modelo de usuario inválido para modificación de clave");
                return BadRequest("Modelo Invalido");
            }

            try
            {
                _logger.Information($"Modificando clave del usuario con ID: {pUsuario.Id}");
                gObjUsuarioLN.ModClaveUsuario(pUsuario.Id, pUsuario.Clave);
                _logger.Information($"Clave del usuario con ID {pUsuario.Id} modificada exitosamente");
                return Ok();
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        [Route("[action]")]
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

        [Route("[action]")]
        [HttpPost]
        public IActionResult eliminaCookie()
        {
            _logger.Information("Eliminando cookie de sesión");
            Response.Cookies.Delete("Token");

            return Ok(new { message = "Cookie eliminada exitosamente" });
        }

    }
}

