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
    public class KardexController : Controller
    {

        public IConfiguration lConfiguration;

        private readonly IKardexLN gObjKardexLN;

        public Excepciones gObjExcepciones = new Excepciones();

        public KardexController(IConfiguration lConfig)
        {
            lConfiguration = lConfig;
            string? lCadenaConexcion = lConfiguration.GetConnectionString("LoteriaBD");

            gObjKardexLN = new KardexLN(lConfiguration);
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
        public ActionResult<List<Kardex>> RecKardex()
        {
            List<Kardex> lObjRespuesta = new List<Kardex>();

            try
            {
                lObjRespuesta = gObjKardexLN.RecKardex();

                var Kardex = lObjRespuesta.Select(u => new Kardex
                {
                    Id = u.Id,
                    Serie = u.Serie,
                    Numero = u.Numero,
                    Nombre = u.Nombre,
                    Monto = u.Monto,
                    IdUsuario = u.IdUsuario
                }).ToList();

                return Ok(Kardex); // Retorna un HTTP 200 con la lista de Kardexs.
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        //[Authorize]
        [Route("[action]")]
        [HttpPost]
        public IActionResult? RecKardexXId([FromBody] Kardex pKardex)
        {
            try
            {
                // Llamada al método para obtener el kardex por su ID
                var lObjRespuesta = gObjKardexLN.RecKardexXId(pKardex.Id);

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
        public IActionResult InsKardex([FromBody] Kardex pKardex)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo Invalido");

            try
            {
                gObjKardexLN.InsKardex(pKardex);
                return CreatedAtAction(nameof(RecKardexXId), new { pIdKardex = pKardex.Id }, pKardex); // Retorna 201 Created, Mejor por convencion de REST API
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        //[Authorize]
        [Route("[action]")]
        [HttpPut]
        public IActionResult ModKardex([FromBody] Kardex pKardex)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo Invalido");

            try
            {
                gObjKardexLN.ModKardex(pKardex);
                return Ok(pKardex);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

        //[Authorize]
        [Route("[action]")]
        [HttpDelete]
        public IActionResult DelKardex([FromBody] int IdKardex)
        {
            try
            {
                var lKardex = gObjKardexLN.RecKardexXId(IdKardex);
                if (lKardex == null)
                {
                    return BadRequest("Kardex No Encontrado");
                }
                gObjKardexLN.DelKardex(lKardex);
                return Ok(lKardex);
            }
            catch (Exception lEx)
            {
                return ManejoError(lEx);
            }
        }

    }
}
