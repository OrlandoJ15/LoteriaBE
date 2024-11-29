using Entidades.Models;
using LogicaNegocio.Implementacion;
using LogicaNegocio.Interfaz;
using MetodosComunes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoteriaWebApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class DetalleSorteoController : Controller
    {

        public IConfiguration lConfiguration;

        private readonly IDetalleSorteoLN gObjDetalleSorteoLN;

        public Excepciones gObjExcepciones = new Excepciones();

        public DetalleSorteoController(IConfiguration lConfig)
        {
            lConfiguration = lConfig;
            string? lCadenaConexcion = lConfiguration.GetConnectionString("LoteriaBD");

            gObjDetalleSorteoLN = new DetalleSorteoLN(lConfiguration);
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

        //[Authorize]
        [Route("[action]")]
        [HttpGet]
        public ActionResult<List<DetalleSorteo>> RecDetalleSorteo()
        {
            List<DetalleSorteo> lObjRespuesta = new List<DetalleSorteo>();

            try
            {
                lObjRespuesta = gObjDetalleSorteoLN.RecDetalleSorteo();

                var DetalleSorteo = lObjRespuesta.Select(u => new DetalleSorteo
                {
                    Id = u.Id,
                    IdSorteo = u.IdSorteo,
                    Numero = u.Numero,
                    Monto = u.Monto
                }).ToList();

                return Ok(DetalleSorteo); // Retorna un HTTP 200 con la lista de DetalleSorteos.
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        //[Authorize]
        [Route("[action]")]
        [HttpPost]
        public IActionResult? RecDetalleSorteoXId([FromBody] DetalleSorteo pDetalleSorteo)
        {
            try
            {
                // Llamada al método para obtener el usuario por su ID
                var lObjRespuesta = gObjDetalleSorteoLN.RecDetalleSorteoXId(pDetalleSorteo.Id);

                return HandleResponse(lObjRespuesta);

            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        //[Authorize]
        [Route("[action]")]
        [HttpPost]
        public IActionResult InsDetalleSorteo([FromBody] DetalleSorteo pDetalleSorteo)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo Invalido");

            try
            {
                gObjDetalleSorteoLN.InsDetalleSorteo(pDetalleSorteo);
                return CreatedAtAction(nameof(RecDetalleSorteoXId), new { pIdDetalleSorteo = pDetalleSorteo.Id }, pDetalleSorteo); // Retorna 201 Created, Mejor por convencion de REST API
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        //[Authorize]
        [Route("[action]")]
        [HttpPut]
        public IActionResult ModDetalleSorteo([FromBody] DetalleSorteo pDetalleSorteo)
        {
            if (!ModelState.IsValid)
                return BadRequest("<odelo Invalido");

            try
            {
                gObjDetalleSorteoLN.ModDetalleSorteo(pDetalleSorteo);
                return Ok(pDetalleSorteo);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        //[Authorize]
        [Route("[action]")]
        [HttpDelete]
        public IActionResult DelDetalleSorteo([FromBody] int IdDetalleSorteo)
        {
            try
            {
                var lDetalleSorteo = gObjDetalleSorteoLN.RecDetalleSorteoXId(IdDetalleSorteo);
                if (lDetalleSorteo == null)
                {
                    return BadRequest("DetalleSorteo No Encontrado");
                }
                gObjDetalleSorteoLN.DelDetalleSorteo(lDetalleSorteo);
                return Ok(lDetalleSorteo);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }
    }
}
