using System;

namespace PizzariaAPI.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int MesaId { get; set; }
        public int UsuarioId { get; set; }
        public string Status { get; set; }
        public DateTime DataAbertura { get; set; }
    }
}