using Entidades.Models;
using LogicaNegocio.Implementacion;
using LogicaNegocio.Interfaz;
using MetodosComunes;
using Microsoft.AspNetCore.Mvc;

namespace LoteriaWebApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class SorteoController : Controller
    {

        public IConfiguration lConfiguration;

        private readonly ISorteoLN gObjSorteoLN;

        public Excepciones gObjExcepciones = new Excepciones();

        public SorteoController(IConfiguration lConfig)
        {
            lConfiguration = lConfig;
            string? lCadenaConexcion = lConfiguration.GetConnectionString("LoteriaBD");

            gObjSorteoLN = new SorteoLN(lConfiguration);
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
        public ActionResult<List<Sorteo>> RecSorteo()
        {
            List<Sorteo> lObjRespuesta = new List<Sorteo>();

            try
            {
                lObjRespuesta = gObjSorteoLN.RecSorteo();

                var Sorteo = lObjRespuesta.Select(u => new Sorteo
                {
                    IdSorteo = u.IdSorteo,
                    Nombre = u.Nombre,
                    Numero = u.Numero,
                    Monto = u.Monto,
                    IdUsuario = u.IdUsuario,
                    IdTipoSorteo = u.IdTipoSorteo
                }).ToList();

                return Ok(Sorteo); // Retorna un HTTP 200 con la lista de Sorteos.
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult? RecSorteoXId([FromBody] Sorteo pSorteo)
        {
            try
            {
                // Llamada al método para obtener el usuario por su ID
                var lObjRespuesta = gObjSorteoLN.RecSorteoXId(pSorteo.IdSorteo);

                return HandleResponse(lObjRespuesta);

            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult InsSorteo([FromBody] Sorteo pSorteo)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo Invalido");

            try
            {
                gObjSorteoLN.InsSorteo(pSorteo);
                return CreatedAtAction(nameof(RecSorteoXId), new { pIdSorteo = pSorteo.IdSorteo }, pSorteo); // Retorna 201 Created, Mejor por convencion de REST API
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        [Route("[action]")]
        [HttpPut]
        public IActionResult ModSorteo([FromBody] Sorteo pSorteo)
        {
            if (!ModelState.IsValid)
                return BadRequest("<odelo Invalido");

            try
            {
                gObjSorteoLN.ModSorteo(pSorteo);
                return Ok(pSorteo);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        [Route("[action]")]
        [HttpDelete]
        public IActionResult DelSorteo([FromBody] int IdSorteo)
        {

            try
            {
                var lSorteo = gObjSorteoLN.RecSorteoXId(IdSorteo);
                if (lSorteo == null)
                {
                    return BadRequest("Sorteo No Encontrado");
                }
                gObjSorteoLN.DelSorteo(lSorteo);
                return Ok(lSorteo);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

    }
}
