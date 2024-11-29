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
    public class TipoSorteoController : Controller
    {

        public IConfiguration lConfiguration;

        private readonly ITipoSorteoLN gObjTipoSorteoLN;

        public Excepciones gObjExcepciones = new Excepciones();

        public TipoSorteoController(IConfiguration lConfig)
        {
            lConfiguration = lConfig;
            string? lCadenaConexcion = lConfiguration.GetConnectionString("LoteriaBD");

            gObjTipoSorteoLN = new TipoSorteoLN(lConfiguration);
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
        public ActionResult<List<TipoSorteo>> RecTipoSorteo()
        {
            List<TipoSorteo> lObjRespuesta = new List<TipoSorteo>();

            try
            {
                lObjRespuesta = gObjTipoSorteoLN.RecTipoSorteo();

                var TipoSorteo = lObjRespuesta.Select(u => new TipoSorteo
                {
                    Id = u.Id,
                    NumeroGanador = u.NumeroGanador,
                    IdTipoSorteoGeneral = u.IdTipoSorteoGeneral,
                    Fecha = u.Fecha,
                    NombreTipoSorteoGeneral = u.NombreTipoSorteoGeneral,
                }).ToList();

                return Ok(TipoSorteo); // Retorna un HTTP 200 con la lista de TipoSorteos.
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        //[Authorize]
        [Route("[action]")]
        [HttpPost]
        public IActionResult? RecTipoSorteoXId([FromBody] TipoSorteo pTipoSorteo)
        {
            try
            {
                // Llamada al método para obtener el usuario por su ID
                var lObjRespuesta = gObjTipoSorteoLN.RecTipoSorteoXId(pTipoSorteo.Id);

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
        public IActionResult InsTipoSorteo([FromBody] TipoSorteo pTipoSorteo)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo Invalido");

            try
            {
                gObjTipoSorteoLN.InsTipoSorteo(pTipoSorteo);
                return CreatedAtAction(nameof(RecTipoSorteoXId), new { pIdTipoSorteo = pTipoSorteo.Id }, pTipoSorteo); // Retorna 201 Created, Mejor por convencion de REST API
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }
        //[Authorize]
        [Route("[action]")]
        [HttpPut]
        public IActionResult ModTipoSorteo([FromBody] TipoSorteo pTipoSorteo)
        {
            if (!ModelState.IsValid)
                return BadRequest("<odelo Invalido");

            try
            {
                gObjTipoSorteoLN.ModTipoSorteo(pTipoSorteo);
                return Ok(pTipoSorteo);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        //[Authorize]
        [Route("[action]")]
        [HttpDelete]
        public IActionResult DelTipoSorteo([FromBody] int IdTipoSorteo)
        {

            try
            {
                var lTipoSorteo = gObjTipoSorteoLN.RecTipoSorteoXId(IdTipoSorteo);
                if (lTipoSorteo == null)
                {
                    return BadRequest("TipoSorteo No Encontrado");
                }
                gObjTipoSorteoLN.DelTipoSorteo(lTipoSorteo);
                return Ok(lTipoSorteo);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }



        //[Authorize]
        [Route("[action]")]
        [HttpPost]
        public IActionResult? RecIdTipoSorteoFromTipoSorteoGeneral([FromBody] TipoSorteoGeneral pTipoSorteoGeneral)
        {
            try
            {
                // Llamada al método para obtener el usuario por su ID
                var lObjRespuesta = gObjTipoSorteoLN.RecIdTipoSorteoFromTipoSorteoGeneral(pTipoSorteoGeneral.Id);

                return HandleResponse(lObjRespuesta);

            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }


    }
}
