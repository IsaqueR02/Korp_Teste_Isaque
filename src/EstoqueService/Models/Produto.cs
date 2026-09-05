using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;

namespace EstoqueService.Models
{
    public class Produto
    {
        [Key]
        public int Codigo { get; set; }
        public string? Descricao { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; }
    }
}
