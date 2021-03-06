using System;
using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using FunctionAppAcoes.Data;
using FunctionAppAcoes.Models;
using FunctionAppAcoes.Validators;

namespace FunctionAppAcoes
{
    public class ProcessarAcoes
    {
        private readonly AcoesContext _context;

        public ProcessarAcoes(AcoesContext context)
        {
            _context = context;
        }

        [FunctionName("ProcessarAcoes")]
        public void Run(
            [RabbitMQTrigger("queue-acoes", ConnectionStringSetting = "RabbitMQConnection")] string inputMessage,
            ILogger log)
        {
            log.LogInformation($"ProcessarAcoes - Dados: {inputMessage}");

            DadosAcao dadosAcao = null;
            try
            {
                dadosAcao = JsonSerializer.Deserialize<DadosAcao>(inputMessage,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch
            {
                log.LogError("ProcessarAcoes - Erro durante a deserializacao!");
            }

            if (dadosAcao != null)
            {
                var validationResult = new AcaoValidator().Validate(dadosAcao);
                if (validationResult.IsValid)
                {
                    log.LogInformation($"ProcessarAcoes - Dados pos formatacao: {JsonSerializer.Serialize(dadosAcao)}");

                    _context.Acoes.Add(new Acao()
                    {
                        Codigo = dadosAcao.Codigo,
                        DataReferencia = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        Valor = dadosAcao.Valor.Value
                    });
                    _context.SaveChanges();

                    log.LogInformation("ProcessarAcoes - Acao registrada com sucesso!");
                }
                else
                {
                    log.LogError("ProcessarAcoes - Dados invalidos para a Acao");
                    foreach (var error in validationResult.Errors)
                        log.LogError($"ProcessarAcoes - {error.ErrorMessage}");
                }
            }
        }        
    }
}