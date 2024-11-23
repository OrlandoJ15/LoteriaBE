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

        //[Authorize]
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
                    Id = u.Id,
                    IdUsuario = u.IdUsuario,
                    IdTipoSorteo = u.IdTipoSorteo,
                    NombreUsuario = u.NombreUsuario,
                    NombreTipoSorteoGeneral = u.NombreTipoSorteoGeneral,
                    FechaTipoSorteo = u.FechaTipoSorteo,
                }).ToList();

                return Ok(Sorteo); // Retorna un HTTP 200 con la lista de Sorteos.
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        //[Authorize]
        [Route("[action]")]
        [HttpPost]
        public IActionResult? RecSorteoXId([FromBody] Sorteo pSorteo)
        {
            try
            {
                // Llamada al método para obtener el usuario por su ID
                var lObjRespuesta = gObjSorteoLN.RecSorteoXId(pSorteo.Id);

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
        public IActionResult InsSorteo([FromBody] Sorteo pSorteo)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo Invalido");

            try
            {
                gObjSorteoLN.InsSorteo(pSorteo);
                return CreatedAtAction(nameof(RecSorteoXId), new { pIdSorteo = pSorteo.Id }, pSorteo); // Retorna 201 Created, Mejor por convencion de REST API
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        //[Authorize]
        [Route("[action]")]
        [HttpPut]
        public IActionResult ModSorteo([FromBody] Sorteo pSorteo)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo Invalido");

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

        //[Authorize]
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





        //[Authorize]
        [Route("[action]")]
        [HttpPost]
        public IActionResult? RecIdSorteoFromParametro()
        {
            try
            {
                // Llamada al método para obtener el usuario por su ID
                var lObjRespuesta = gObjSorteoLN.RecIdSorteoFromParametro();

                return HandleResponse(lObjRespuesta);

            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

    }
}
