using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Business;

namespace AplicacaoCondominial.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        IConfiguracaoBusiness _configuracaoBusiness;

        public SampleDataController(IConfiguracaoBusiness configuracaoBusiness)
        {
            _configuracaoBusiness = configuracaoBusiness;
        }

        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<Core.Business.DTOs.Administradora>> Administradoras()
        {
            return (await _configuracaoBusiness.ListarAdministradorasAsync()).Result;
        }
    }
}
