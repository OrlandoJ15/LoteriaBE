using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Entidades.Models;
using LogicaNegocio.Implementacion;
using LogicaNegocio.Interfaz;
using MetodosComunes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoteriaWebApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IConfiguration lConfiguration;

        private readonly IUsuarioLN gObjUsuarioLN;

        private readonly Excepciones gObjExcepciones = new Excepciones();

        public UsuarioController(IConfiguration lConfig)
        {
            lConfiguration = lConfig;
            string? lCadenaConexcion = lConfiguration.GetConnectionString("LoteriaBD");

            gObjUsuarioLN = new UsuarioLN(lConfiguration);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Hello" });
        }

        //METODO PARA GENERAR EL TOKEN JWT

        private string GenearJwtToken(Usuario pUsuario)
        {

            var keyVaultUrl = lConfiguration["AzureKeyVault:VaultUrl"];
            
            //var secretName = lConfiguration["Jwt:KeyName"];

            // Recuperar la clave desde el Key Vault
            var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
            var secret = client.GetSecret("JwtKey").Value.Value;
            //var jwtSigningKey = secret.Value.Value;



            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, pUsuario.NombreUsuario), // Nombre de usuario como 'sub'
                new Claim("Id", pUsuario.Id.ToString()), // ID del usuario
                new Claim("Rol", pUsuario.Rol.ToString()), // Rol del usuario
                new Claim(JwtRegisteredClaimNames.Iss, lConfiguration["Jwt:Issuer"]), // Emisor del token
                new Claim(JwtRegisteredClaimNames.Aud, lConfiguration["Jwt:Audience"]), // Audiencia
                new Claim("Correo", pUsuario.Correo), // Agregar correo si lo necesitas
                new Claim("FechaCreacion", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) // Ejemplo de claim adicional
            };

            //test
            /*
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub , pUsuario.NombreUsuario),    
                new Claim("Id", pUsuario.Id.ToString()),
                new Claim("Rol", pUsuario.Rol.ToString())
            };
            */



            var token = new JwtSecurityToken(
                issuer: lConfiguration["Jwt:Issuer"], // Emisor
                audience: lConfiguration["Jwt:Audience"], // Audiencia
                claims: claims, // Aquí pasamos los claims
                expires: DateTime.Now.AddDays(2), // Expiración del token
                signingCredentials: credentials // Firmado con las credenciales
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }


        //Metodo para manejar la excepcion y devulocion de status de error
        private ActionResult ManejoError(Exception Ex)
        {
            gObjExcepciones.LogError(Ex);
            return StatusCode(StatusCodes.Status500InternalServerError, Ex.Message);
        }


        private IActionResult HandleResponse<T>(T response)
        {
            if (response == null)

            {
                return new JsonResult(null);// Retorna 404 Not Found
            }
            return Ok(response); // Retorna 200 OK con la respuesta
        }

        // Endpoint de autenticación para generar token

        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public IActionResult Login([FromBody] Usuario pUsuario)
        {
            if (pUsuario == null || string.IsNullOrEmpty(pUsuario.Clave))
                return BadRequest("Credenciales Invalidas");

            try
            {
                var user = gObjUsuarioLN.ValidarLoginUsuario(pUsuario.Id, pUsuario.Clave);
                if (user == null)
                    return Unauthorized("Usuario o Clave Incorrecta");

                var token = GenearJwtToken(user);

                /*
                HttpContext.Response.Cookies.Append("Token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    //Path = "/Login",
                    Expires = DateTimeOffset.Now.AddHours(2),
                });
                */
         
                //return Ok(new { Message = "Inicio de sesión exitoso actualizado" });
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
                lObjRespuesta = gObjUsuarioLN.RecUsuario();

                var Usuario = lObjRespuesta.Select(u => new Usuario
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    NombreUsuario = u.NombreUsuario,
                    Rol = u.Rol,
                    Correo = u.Correo
                }).ToList();

                return Ok(Usuario); // Retorna un HTTP 200 con la lista de usuarios.
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
                // Llamada al método para obtener el usuario por su ID
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
                return BadRequest("Modelo Invalido");

            try
            {
                gObjUsuarioLN.InsUsuario(pUsuario);
                return CreatedAtAction(nameof(RecUsuarioXId), new { pIdUsuario = pUsuario.Id }, pUsuario); // Retorna 201 Created, Mejor por convencion de REST API
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
                return BadRequest("Modelo Invalido");

            try
            {
                gObjUsuarioLN.ModUsuario(pUsuario);
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
                var lUsuario = gObjUsuarioLN.RecUsuarioXId(IdUsuario);
                if (lUsuario == null)
                {
                    return BadRequest("Usuario No Encontrado");
                }
                gObjUsuarioLN.DelUsuario(lUsuario);
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
                return BadRequest("Datos inválidos o incompletos");

            try
            {
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
                return BadRequest("Modelo Invalido");

            try
            {
                gObjUsuarioLN.ModClaveUsuario(pUsuario.Id, pUsuario.Clave);
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
            // Esto asegura que la cookie JWT sea eliminada
            Response.Cookies.Append("Token", "", new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1), // Se asegura de que la cookie expire inmediatamente
                HttpOnly = true,
                Secure = true, // Cambiar a 'false' si estás trabajando en desarrollo sin HTTPS
                SameSite = SameSiteMode.Lax // Ajusta según sea necesario
            });

            return Ok(new { message = "Usuario deslogueado exitosamente" });
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult eliminaCookie()
        {
            // Esto asegura que la cookie JWT sea eliminada
            Response.Cookies.Delete("Token");

            return Ok(new { message = "Cookie eliminada exitosamente" });
        }

    }
}

