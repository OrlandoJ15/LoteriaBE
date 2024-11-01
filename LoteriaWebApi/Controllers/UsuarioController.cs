using Entidades.Models;
using LogicaNegocio.Implementacion;
using LogicaNegocio.Interfaz;
using MetodosComunes;
using Microsoft.AspNetCore.Mvc;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

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


        //METODO PARA GENERAR EL TOKEN JWT
        
        private string GenearJwtToken(Usuario pUsuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(lConfiguration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub , pUsuario.NombreUsuario),
                new Claim("IdUsuario", pUsuario.IdUsuario.ToString()),
                new Claim("Rol", pUsuario.Rol.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: lConfiguration["Jwt:Issuer"],
                audience: lConfiguration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

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
                var user = gObjUsuarioLN.ValidarLoginUsuario(pUsuario.IdUsuario, pUsuario.Clave);
                if (user == null)
                    return Unauthorized("Usuario o Clave Incorrecta");

                var token = GenearJwtToken(user);
                return Ok(new { Token = token });
            }catch (Exception ex)
            {
                return ManejoError(ex);
            }
        }

        [Authorize]
        [Route("[action]")]
        [HttpGet]
        public ActionResult<List<Usuario>> RecUsuario()
        {
            List<Usuario> lObjRespuesta = new List<Usuario>();

            try
            {
                lObjRespuesta = gObjUsuarioLN.RecUsuario();

                var Usuario = lObjRespuesta.Select(u => new Usuario
                {
                    IdUsuario = u.IdUsuario,
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



        [Route("[action]")]
        [HttpPost]
        public IActionResult? RecUsuarioXId([FromBody] Usuario pUsuario)
        {
            try
            {
                // Llamada al método para obtener el usuario por su ID
                var lObjRespuesta = gObjUsuarioLN.RecUsuarioXId(pUsuario.IdUsuario);

                return HandleResponse(lObjRespuesta);

            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }


        [Route("[action]")]
        [HttpPost]
        public IActionResult InsUsuario([FromBody] Usuario pUsuario)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo Invalido");

            try
            {
                gObjUsuarioLN.InsUsuario(pUsuario);
                return CreatedAtAction(nameof(RecUsuarioXId), new { pIdUsuario = pUsuario.IdUsuario }, pUsuario); // Retorna 201 Created, Mejor por convencion de REST API
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        [Route("[action]")]
        [HttpPut]
        public IActionResult ModUsuario([FromBody] Usuario pUsuario)
        {
            if (!ModelState.IsValid)
                return BadRequest("<odelo Invalido");

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
                var lUsuario = gObjUsuarioLN.ValidarLoginUsuario(pUsuario.IdUsuario, pUsuario.Clave);
                return HandleResponse(lUsuario); 
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        [Route("[action]")]
        [HttpPut]
        public IActionResult ModClaveUsuario([FromBody] Usuario pUsuario)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(pUsuario.Clave))
                return BadRequest("Modelo Invalido");

            try
            {
                gObjUsuarioLN.ModClaveUsuario(pUsuario.IdUsuario, pUsuario.Clave);
                return Ok();
                
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }
    }
}

