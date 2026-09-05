using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FaturamentoService.Models
{
    public class NotaFiscal
    {
        [Key]
        public int NumeroSequencial { get; set; }
        public enum statusNotaFiscal
        {
            Aberta,
            Fechada
        }
        public statusNotaFiscal Status { get; set; }
        public ICollection<ItemNotaFiscal> Itens { get; set; } = new List<ItemNotaFiscal>();
    }
}
