using System;

namespace FunctionAppAcoes.Models
{
    public class Acao
    {
        public int? Id { get; set; }
        public string Codigo { get; set; }
        public string DataReferencia { get; set; }
        public decimal Valor { get; set; }
    }
}