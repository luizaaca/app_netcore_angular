using Core.Business;
using Core.Business.DTOs;
using Core.Business.Wraps;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AplicacaoCondominial.Controllers
{
    [Produces("application/json")]
    [Route("api/Comunicacao")]
    public class ComunicacaoController : Controller
    {
        IComunicacaoBusiness _comunicacaoBusiness;

        public ComunicacaoController(IComunicacaoBusiness comunicacaoBusiness)
        {
            _comunicacaoBusiness = comunicacaoBusiness;
        }


        [HttpPost]
        public async Task<IActionResult> Comunicado(Comunicado comunicado)
        {
            var response = await _comunicacaoBusiness
                                    .EnviarComunicadoAsync(
                                        new BaseRequest<Comunicado>(comunicado));

            if (!response.Success)
                return StatusCode(500, response.Message);

            return Ok();
        }
    }
}