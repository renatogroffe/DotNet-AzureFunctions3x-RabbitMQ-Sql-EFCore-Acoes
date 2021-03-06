using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FunctionAppAcoes.Data;

[assembly: FunctionsStartup(typeof(FunctionAppAcoes.Startup))]
namespace FunctionAppAcoes
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddEntityFrameworkSqlServer()
                .AddDbContext<AcoesContext>(
                    options => options.UseSqlServer(
                        Environment.GetEnvironmentVariable("BaseAcoes")));
        }
    }
}