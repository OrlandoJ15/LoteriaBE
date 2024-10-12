
using Microsoft.AspNetCore.Mvc;
using LogicaNegocio.Interfaz;
using LogicaNegocio.Implementacion;
using Entidades.Models;
using NLog;

namespace LoteriaWebApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {



        public IConfiguration lConfiguration;

        private readonly IUsuarioLN gObjUsuarioLN;
        private readonly Logger gObjError = LogManager.GetCurrentClassLogger();

        public UsuarioController(IConfiguration lConfig)
        {
            lConfiguration = lConfig;
            string? lCadenaConexcion = lConfiguration.GetConnectionString("LoteriaBD");
            
            gObjUsuarioLN = new UsuarioLN(lConfiguration);
        }


        [Route("[action]")]
        [HttpGet]
        public ActionResult<List<Usuario>> RecUsuario()
        {
            List<Usuario> lObjRespuesta = new List<Usuario>();

            try
            {
                lObjRespuesta = gObjUsuarioLN.RecUsuario();

                // Convierte la lista de Usuario a una lista de UserDto excluyendo el campo 'Clave'
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
                // Obtener el nombre del método actual de forma segura
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible"; // Uso del operador de coalescencia nula

                // Registrar el error
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);

                // Retornar un InternalServerError con un código HTTP 500
                return StatusCode(StatusCodes.Status500InternalServerError, lEx.Message);
            }
        }

        [Route("[action]/{pIdUsuario}")]
        [HttpGet]
        public IActionResult RecUsuarioXId(int pIdUsuario)
        {
            try
            {
                // Llamada al método para obtener el usuario por su ID
                var lObjRespuesta = gObjUsuarioLN.RecUsuarioXId(pIdUsuario);

                // Si no se encuentra el usuario, devolver 200 OK con null
                if (lObjRespuesta == null)
                {
                    return new JsonResult(null); // Retorna 200 OK con una nuevo json para aseurar que el front reciba null 
                }

                // Si el usuario se encuentra, devolver 200 OK con el objeto usuario
                return Ok(lObjRespuesta);
            }
            catch (Exception lEx)
            {
                // Obtener el nombre del método actual de forma segura
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible"; // Uso del operador de coalescencia nula

                // Registrar el error
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);

                // Retornar un InternalServerError con un código HTTP 500
                return StatusCode(StatusCodes.Status500InternalServerError, lEx.Message);
            }
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult InsUsuario([FromBody] Usuario pUsuario)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    gObjUsuarioLN.InsUsuario(pUsuario);
                    return Ok(pUsuario);
                }
                else
                {
                    return BadRequest("Modelo Invalido");
                }
            }
            catch (Exception lEx)
            {

                // Obtener el nombre del método actual de forma segura
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible"; // Uso del operador de coalescencia nula

                // Registrar el error
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);

                // Retornar un InternalServerError con un código HTTP 500
                return StatusCode(StatusCodes.Status500InternalServerError, lEx.Message);
            }
        }

        [Route("[action]")]
        [HttpPut]
        public IActionResult ModUsuario([FromBody] Usuario pUsuario)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    gObjUsuarioLN.ModUsuario(pUsuario);
                    return Ok(pUsuario);
                }
                else
                {
                    return BadRequest("Modelo Invalido");
                }
            }
            catch (Exception lEx)
            {
                // Obtener el nombre del método actual de forma segura
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible"; // Uso del operador de coalescencia nula

                // Registrar el error
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);

                // Retornar un InternalServerError con un código HTTP 500
                return StatusCode(StatusCodes.Status500InternalServerError, lEx.Message);
            }
        }



        [Route("[action]")]
        [HttpDelete]
        public IActionResult DelUsuario([FromBody] int IdUsuario)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var lUsuario = gObjUsuarioLN.RecUsuarioXId(IdUsuario);
                    if (lUsuario != null)
                    {
                        gObjUsuarioLN.DelUsuario(lUsuario);
                        return Ok(lUsuario);
                    }
                    else
                    {
                        return BadRequest("Usuario No Encontrado");
                    }
                }
                else
                {
                    return BadRequest("Modelo Invalido");
                }
            }
            catch (Exception lEx)
            {
                // Obtener el nombre del método actual de forma segura
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible"; // Uso del operador de coalescencia nula

                // Registrar el error
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);

                // Retornar un InternalServerError con un código HTTP 500
                return StatusCode(StatusCodes.Status500InternalServerError, lEx.Message);
            }
        }

        [Route("[action]/{pId}/{pClave}")]

        [HttpGet]
        public IActionResult ValidarUsuarioLogin(int pId, string pClave)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var lUsuario = gObjUsuarioLN.ValidarLoginUsuario(pId, pClave);
                    if (lUsuario != null)
                    {
                        return Ok(lUsuario);
                    }
                    else
                    {
                        return new JsonResult(null); // Retorna 200 OK con una nuevo json para aseurar que el front reciba null
                    }
                }
                else
                {
                    return BadRequest("Modelo Invalido");
                }
            }
            catch (Exception lEx)
            {
                // Obtener el nombre del método actual de forma segura
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible"; // Uso del operador de coalescencia nula

                // Registrar el error
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);

                // Retornar un InternalServerError con un código HTTP 500
                return StatusCode(StatusCodes.Status500InternalServerError, lEx.Message);
            }
        }

        [Route("[action]")]
        [HttpPut]
        public IActionResult ModClaveUsuario([FromBody] int pId, string pClave)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    gObjUsuarioLN.ModClaveUsuario(pId, pClave);
                    return Ok();
                }
                else
                {
                    return BadRequest("Modelo Invalido");
                }
            }
            catch (Exception lEx)
            {
                // Obtener el nombre del método actual de forma segura
                var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
                string methodName = methodInfo?.ToString() ?? "Método no disponible"; // Uso del operador de coalescencia nula

                // Registrar el error
                gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                                "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                                ". Método: " + methodName);

                // Retornar un InternalServerError con un código HTTP 500
                return StatusCode(StatusCodes.Status500InternalServerError, lEx.Message);
            }
        }

    }
}

/*

using Microsoft.AspNetCore.Mvc;
using LogicaNegocio.Interfaz;
using LogicaNegocio.Implementacion;
using Entidades.Models;
using NLog;

namespace LoteriaWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioLN gObjUsuarioLN;
        private readonly Logger gObjError = LogManager.GetCurrentClassLogger();

        // Constructor que recibe una instancia de IUsuarioLN
        //public UsuarioController(IUsuarioLN usuarioLN)
        //{
        //    gObjUsuarioLN = usuarioLN ?? throw new ArgumentNullException(nameof(usuarioLN));
        //}


        public UsuarioController(IConfiguration lConfig, IUsuarioLN gObjUsuarioLN)
        {
            lConfiguration = lConfig;
            this.gObjUsuarioLN = gObjUsuarioLN; // Usar la instancia inyectada
            string? lCadenaConexcion = lConfiguration.GetConnectionString("gCnnBD");
            if (string.IsNullOrEmpty(lCadenaConexcion))
            {
                throw new InvalidOperationException("La cadena de conexión no está configurada.");
            }
        }

        [Route("[action]")]
        [HttpGet]
        public ActionResult<List<Usuario>> RecUsuario()
        {
            try
            {
                // Obtener lista de usuarios
                var usuarios = gObjUsuarioLN.RecUsuario();

                // Retornar la lista de usuarios
                return Ok(usuarios.Select(u => new Usuario
                {
                    IdUsuario = u.IdUsuario,
                    Nombre = u.Nombre,
                    NombreUsuario = u.NombreUsuario,
                    Rol = u.Rol,
                    Correo = u.Correo
                }).ToList());
            }
            catch (Exception lEx)
            {
                LogError(lEx);
                return StatusCode(StatusCodes.Status500InternalServerError, lEx.Message);
            }
        }

        [Route("[action]/{pIdUsuario}")]
        [HttpGet]
        public IActionResult RecUsuarioXId(int pIdUsuario)
        {
            try
            {
                // Obtener usuario por ID
                var usuario = gObjUsuarioLN.RecUsuarioXId(pIdUsuario);

                if (usuario == null)
                {
                    return NotFound("Usuario no encontrado.");
                }

                return Ok(usuario);
            }
            catch (Exception lEx)
            {
                LogError(lEx);
                return StatusCode(StatusCodes.Status500InternalServerError, lEx.Message);
            }
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult InsUsuario([FromBody] Usuario pUsuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Modelo Invalido");
            }

            try
            {
                gObjUsuarioLN.InsUsuario(pUsuario);
                return CreatedAtAction(nameof(RecUsuarioXId), new { pIdUsuario = pUsuario.IdUsuario }, pUsuario);
            }
            catch (Exception lEx)
            {
                LogError(lEx);
                return StatusCode(StatusCodes.Status500InternalServerError, lEx.Message);
            }
        }

        [Route("[action]")]
        [HttpPut]
        public IActionResult ModUsuario([FromBody] Usuario pUsuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Modelo Invalido");
            }

            try
            {
                gObjUsuarioLN.ModUsuario(pUsuario);
                return NoContent(); // Retorna 204 No Content
            }
            catch (Exception lEx)
            {
                LogError(lEx);
                return StatusCode(StatusCodes.Status500InternalServerError, lEx.Message);
            }
        }

        [Route("[action]")]
        [HttpDelete("{idUsuario}")]
        public IActionResult DelUsuario(int idUsuario)
        {
            try
            {
                var usuario = gObjUsuarioLN.RecUsuarioXId(idUsuario);
                if (usuario == null)
                {
                    return NotFound("Usuario No Encontrado");
                }

                gObjUsuarioLN.DelUsuario(usuario);
                return NoContent(); // Retorna 204 No Content
            }
            catch (Exception lEx)
            {
                LogError(lEx);
                return StatusCode(StatusCodes.Status500InternalServerError, lEx.Message);
            }
        }

        [Route("[action]/{pId}/{pClave}")]
        [HttpGet]
        public IActionResult ValidarUsuarioLogin(int pId, string pClave)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuarioValido = gObjUsuarioLN.ValidarLoginUsuario(pId, pClave);
                    return Ok(usuarioValido);
                }

                return BadRequest("Modelo Invalido");
            }
            catch (Exception lEx)
            {
                LogError(lEx);
                return StatusCode(StatusCodes.Status500InternalServerError, lEx.Message);
            }
        }

        private void LogError(Exception lEx)
        {
            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            string methodName = methodInfo?.ToString() ?? "Método no disponible";
            gObjError.Error($"SE HA PRODUCIDO UN ERROR. Detalle: {lEx.Message}// {lEx.InnerException?.Message ?? "No Inner Exception"}. Método: {methodName}");
        }
    }
}


*/