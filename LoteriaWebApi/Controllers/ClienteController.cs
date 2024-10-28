using Entidades.Models;
using LogicaNegocio.Implementacion;
using LogicaNegocio.Interfaz;
using MetodosComunes;
using Microsoft.AspNetCore.Mvc;

namespace LoteriaWebApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {

        public IConfiguration lConfiguration;

        private readonly IClienteLN gObjClienteLN;

        public Excepciones gObjExcepciones = new Excepciones();

        public ClienteController(IConfiguration lConfig)
        {
            lConfiguration = lConfig;
            string? lCadenaConexcion = lConfiguration.GetConnectionString("LoteriaBD");

            gObjClienteLN = new ClienteLN(lConfiguration);
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



        [Route("[action]")]
        [HttpGet]
        public ActionResult<List<Cliente>> RecCliente()
        {
            List<Cliente> lObjRespuesta = new List<Cliente>();

            try
            {
                lObjRespuesta = gObjClienteLN.RecCliente();

                var Cliente = lObjRespuesta.Select(u => new Cliente
                {
                    IdCliente = u.IdCliente,
                    Cedula = u.Cedula,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    Telefono = u.Telefono,
                    FechaCreacion = u.FechaCreacion,
                    FechaBorrado = u.FechaBorrado,
                    Bloqueado = u.Bloqueado,
                    NombreUsuario = u.NombreUsuario,
                    Clave = u.Clave,

                }).ToList();

                return Ok(Cliente); // Retorna un HTTP 200 con la lista de usuarios.
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }


        [Route("[action]")]
        [HttpPost]
        public IActionResult? RecClienteXId([FromBody] Cliente pCliente)
        {
            try
            {
                // Llamada al método para obtener el usuario por su ID
                var lObjRespuesta = gObjClienteLN.RecClienteXId(pCliente.IdCliente);

                return HandleResponse(lObjRespuesta);

            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }


        [Route("[action]")]
        [HttpPost]
        public IActionResult InsCliente([FromBody] Cliente pCliente)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo Invalido");

            try
            {
                gObjClienteLN.InsCliente(pCliente);
                return CreatedAtAction(nameof(RecClienteXId), new { pIdCliente = pCliente.IdCliente }, pCliente); // Retorna 201 Created, Mejor por convencion de REST API
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        [Route("[action]")]
        [HttpPut]
        public IActionResult ModCliente([FromBody] Cliente pCliente)
        {
            if (!ModelState.IsValid)
                return BadRequest("<odelo Invalido");

            try
            {
                gObjClienteLN.ModCliente(pCliente);
                return Ok(pCliente);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        [Route("[action]")]
        [HttpDelete]
        public IActionResult DelCliente([FromBody] int IdCliente)
        {

            try
            {
                var lCliente = gObjClienteLN.RecClienteXId(IdCliente);
                if (lCliente == null)
                {
                    return BadRequest("Cliente No Encontrado");
                }
                gObjClienteLN.DelCliente(lCliente);
                return Ok(lCliente);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult? ValidarClienteLogin([FromBody] Cliente pCliente)
        {
            if (!ModelState.IsValid || pCliente == null || string.IsNullOrEmpty(pCliente.Clave))
                return BadRequest("Datos inválidos o incompletos");

            try
            {
                var lCliente = gObjClienteLN.ValidarLoginCliente(pCliente.IdCliente, pCliente.Clave);
                return HandleResponse(lCliente); 
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        [Route("[action]")]
        [HttpPut]
        public IActionResult ModClaveCliente([FromBody] Cliente pCliente)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(pCliente.Clave))
                return BadRequest("Modelo Invalido");

            try
            {
                gObjClienteLN.ModClaveCliente(pCliente.IdCliente, pCliente.Clave);
                return Ok();
                
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }
    }
}

