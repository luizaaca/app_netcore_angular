using Core.Business;
using Core.Business.DTOs;
using Core.Business.Wraps;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AplicacaoCondominial.Controllers
{
    [Produces("application/json")]
    [Route("api/Configuracao")]
    public class ConfiguracaoController : Controller
    {
        IConfiguracaoBusiness _configuracaoBusiness;

        public ConfiguracaoController(IConfiguracaoBusiness configuracaoBusiness)
        {
            _configuracaoBusiness = configuracaoBusiness;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Administradoras()
        {
            var response = await _configuracaoBusiness.ListarAdministradorasAsync();

            if (!response.Success)
                return StatusCode(500, response.Message);

            return Ok(response.Result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Condominios()
        {
            var response = await _configuracaoBusiness.ListarCondominiosAsync();

            if (!response.Success)
                return StatusCode(500, response.Message);

            return Ok(response.Result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Usuarios()
        {
            var response = await _configuracaoBusiness.ListarUsuariosAsync();

            if (!response.Success)
                return StatusCode(500, response.Message);

            return Ok(response.Result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Assuntos()
        {
            var response = await _configuracaoBusiness.ListarAssuntosAsync();

            if (!response.Success)
                return StatusCode(500, response.Message);

            return Ok(response.Result);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Condominio(int id)
        {
            if (id <= 0)
                return BadRequest(id);

            var response = await _configuracaoBusiness.BuscarCondominioAsync(new BaseRequest<int>(id));

            if (!response.Success)
                return StatusCode(500, response.Message);

            return Ok(response.Result);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Usuario(int id)
        {
            if (id <= 0)
                return BadRequest(id);

            var response = await _configuracaoBusiness.BuscarUsuarioAsync(new BaseRequest<int>(id));

            if (!response.Success)
                return StatusCode(500, response.Message);

            return Ok(response.Result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Condominio([FromBody]Condominio model)
        {
            if (model == null)
                return BadRequest(model);

            var response = await _configuracaoBusiness
                .SalvarCondominioAsync(new BaseRequest<Condominio>(model));

            if (!response.Success)
                return StatusCode(500, response.Message);

            return Ok(response.Result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Usuario([FromBody]Usuario model)
        {
            if (model == null)
                return BadRequest(model);

            var response = await _configuracaoBusiness
                .SalvarUsuarioAsync(new BaseRequest<Usuario>(model));

            if (!response.Success)
                return StatusCode(500, response.Message);

            return Ok(response.Result);
        }
    }
}