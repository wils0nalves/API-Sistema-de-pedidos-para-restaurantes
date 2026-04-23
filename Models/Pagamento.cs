using System;

namespace PizzariaAPI.Models
{
    public class Pagamento
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
    }
}