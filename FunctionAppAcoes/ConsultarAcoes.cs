using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FunctionAppAcoes.Data;

namespace FunctionAppAcoes
{
    public class ConsultarAcoes
    {
        private readonly AcoesContext _context;

        public ConsultarAcoes(AcoesContext context)
        {
            _context = context;
        }

        [FunctionName("ConsultarAcoes")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "acoes")] HttpRequest req,
            ILogger log)
        {
            var listaAcoes = _context.Acoes
                .OrderByDescending(a => a.Id).ToArray();
            log.LogInformation(
                $"ConsultarAcoes HTTP trigger - numero atual de lancamentos: {listaAcoes.Count()}");
            return new OkObjectResult(listaAcoes);
        }
    }
}