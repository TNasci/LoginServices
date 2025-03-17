using LoginService.App.Models;
using LoginService.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoginService.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [Authorize]
        public IActionResult Get()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userRole))
            {
                return Unauthorized(new { Message = "N�o foi poss�vel identificar a fun��o do usu�rio." });
            }

            string mensagemBoasVindas = userRole switch
            {
                "Admin" => "Bem-vindo, Administrador!",
                "Doctor" => "Bem-vindo, Doutor!",
                "Pacient" => "Bem-vindo, Paciente!",
                _ => "Bem-vindo, usu�rio!"
            };

            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            return Ok(new { Mensagem = mensagemBoasVindas, Previsoes = forecasts });
        }
    }
}
