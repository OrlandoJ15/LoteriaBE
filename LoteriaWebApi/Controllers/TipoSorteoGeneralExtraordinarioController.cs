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
    public class TipoSorteoGeneralExtraordinarioController : Controller
    {

        public IConfiguration lConfiguration;

        private readonly ITipoSorteoGeneralExtraordinarioLN gObjTipoSorteoGeneralExtraordinarioLN;

        public Excepciones gObjExcepciones = new Excepciones();

        public TipoSorteoGeneralExtraordinarioController(IConfiguration lConfig)
        {
            lConfiguration = lConfig;
            string? lCadenaConexcion = lConfiguration.GetConnectionString("LoteriaBD");

            gObjTipoSorteoGeneralExtraordinarioLN = new TipoSorteoGeneralExtraordinarioLN(lConfiguration);
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
        public ActionResult<List<TipoSorteoGeneralExtraordinario>> RecTipoSorteoGeneralExtraordinario()
        {
            List<TipoSorteoGeneralExtraordinario> lObjRespuesta = new List<TipoSorteoGeneralExtraordinario>();

            try
            {
                lObjRespuesta = gObjTipoSorteoGeneralExtraordinarioLN.RecTipoSorteoGeneralExtraordinario();

                var TipoSorteoGeneralExtraordinario = lObjRespuesta.Select(u => new TipoSorteoGeneralExtraordinario
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Fondo = u.Fondo,
                    PorcentajePago = u.PorcentajePago,
                    FechaInicio = u.FechaInicio,
                    FechaFin = u.FechaFin
                }).ToList();

                return Ok(TipoSorteoGeneralExtraordinario); // Retorna un HTTP 200 con la lista de TipoSorteoGenerals.
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        //[Authorize]
        [Route("[action]")]
        [HttpPost]
        public IActionResult? RecTipoSorteoGeneralExtraordinarioXId([FromBody] TipoSorteoGeneralExtraordinario pTipoSorteoGeneralExtraordinario)
        {
            try
            {
                // Llamada al método para obtener el usuario por su ID
                var lObjRespuesta = gObjTipoSorteoGeneralExtraordinarioLN.RecTipoSorteoGeneralExtraordinarioXId(pTipoSorteoGeneralExtraordinario.Id);

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
        public IActionResult InsTipoSorteoGeneralExtraordinario([FromBody] TipoSorteoGeneralExtraordinario pTipoSorteoGeneralExtraordinario)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo Invalido");

            try
            {
                gObjTipoSorteoGeneralExtraordinarioLN.InsTipoSorteoGeneralExtraordinario(pTipoSorteoGeneralExtraordinario);
                return CreatedAtAction(nameof(RecTipoSorteoGeneralExtraordinarioXId), new { pIdTipoSorteoGeneralExtraordinario = pTipoSorteoGeneralExtraordinario.Id }, pTipoSorteoGeneralExtraordinario); // Retorna 201 Created, Mejor por convencion de REST API
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }
        //[Authorize]
        [Route("[action]")]
        [HttpPut]
        public IActionResult ModTipoSorteoGeneralExtraordinario([FromBody] TipoSorteoGeneralExtraordinario pTipoSorteoGeneralExtraordinario)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo Invalido");

            try
            {
                gObjTipoSorteoGeneralExtraordinarioLN.ModTipoSorteoGeneralExtraordinario(pTipoSorteoGeneralExtraordinario);
                return Ok(pTipoSorteoGeneralExtraordinario);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        //[Authorize]
        [Route("[action]")]
        [HttpDelete]
        public IActionResult DelTipoSorteoGeneralExtraordinario([FromBody] int IdTipoSorteoGeneralExtraordinario)
        {

            try
            {
                var lTipoSorteoGeneralExtraordinario = gObjTipoSorteoGeneralExtraordinarioLN.RecTipoSorteoGeneralExtraordinarioXId(IdTipoSorteoGeneralExtraordinario);
                if (lTipoSorteoGeneralExtraordinario == null)
                {
                    return BadRequest("TipoSorteoGeneral No Encontrado");
                }
                gObjTipoSorteoGeneralExtraordinarioLN.DelTipoSorteoGeneralExtraordinario(lTipoSorteoGeneralExtraordinario);
                return Ok(lTipoSorteoGeneralExtraordinario);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

    }
}
