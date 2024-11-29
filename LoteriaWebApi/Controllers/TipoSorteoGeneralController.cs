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
    public class TipoSorteoGeneralController : Controller
    {

        public IConfiguration lConfiguration;

        private readonly ITipoSorteoGeneralLN gObjTipoSorteoGeneralLN;

        public Excepciones gObjExcepciones = new Excepciones();

        public TipoSorteoGeneralController(IConfiguration lConfig)
        {
            lConfiguration = lConfig;
            string? lCadenaConexcion = lConfiguration.GetConnectionString("LoteriaBD");

            gObjTipoSorteoGeneralLN = new TipoSorteoGeneralLN(lConfiguration);
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
        public ActionResult<List<TipoSorteoGeneral>> RecTipoSorteoGeneral()
        {
            List<TipoSorteoGeneral> lObjRespuesta = new List<TipoSorteoGeneral>();

            try
            {
                lObjRespuesta = gObjTipoSorteoGeneralLN.RecTipoSorteoGeneral();

                var TipoSorteoGeneral = lObjRespuesta.Select(u => new TipoSorteoGeneral
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Fondo = u.Fondo,
                    PorcentajePago = u.PorcentajePago,
                    HoraFin = u.HoraFin,
                }).ToList();

                return Ok(TipoSorteoGeneral); // Retorna un HTTP 200 con la lista de TipoSorteoGenerals.
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        //[Authorize]
        [Route("[action]")]
        [HttpPost]
        public IActionResult? RecTipoSorteoGeneralXId([FromBody] TipoSorteoGeneral pTipoSorteoGeneral)
        {
            try
            {
                // Llamada al método para obtener el usuario por su ID
                var lObjRespuesta = gObjTipoSorteoGeneralLN.RecTipoSorteoGeneralXId(pTipoSorteoGeneral.Id);

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
        public IActionResult InsTipoSorteoGeneral([FromBody] TipoSorteoGeneral pTipoSorteoGeneral)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo Invalido");

            try
            {
                gObjTipoSorteoGeneralLN.InsTipoSorteoGeneral(pTipoSorteoGeneral);
                return CreatedAtAction(nameof(RecTipoSorteoGeneralXId), new { pIdTipoSorteoGeneral = pTipoSorteoGeneral.Id }, pTipoSorteoGeneral); // Retorna 201 Created, Mejor por convencion de REST API
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }
        //[Authorize]
        [Route("[action]")]
        [HttpPut]
        public IActionResult ModTipoSorteoGeneral([FromBody] TipoSorteoGeneral pTipoSorteoGeneral)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo Invalido");

            try
            {
                gObjTipoSorteoGeneralLN.ModTipoSorteoGeneral(pTipoSorteoGeneral);
                return Ok(pTipoSorteoGeneral);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        //[Authorize]
        [Route("[action]")]
        [HttpDelete]
        public IActionResult DelTipoSorteoGeneral([FromBody] int IdTipoSorteoGeneral)
        {

            try
            {
                var lTipoSorteoGeneral = gObjTipoSorteoGeneralLN.RecTipoSorteoGeneralXId(IdTipoSorteoGeneral);
                if (lTipoSorteoGeneral == null)
                {
                    return BadRequest("TipoSorteoGeneral No Encontrado");
                }
                gObjTipoSorteoGeneralLN.DelTipoSorteoGeneral(lTipoSorteoGeneral);
                return Ok(lTipoSorteoGeneral);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

    }
}
