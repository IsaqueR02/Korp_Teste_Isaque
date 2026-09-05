using System;
using System.Collections.Generic;
using System.Linq;
using FaturamentoService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace FaturamentoService.Data
{
    public class FaturamentoDbContext : DbContext
    {
        public DbSet<NotaFiscal> NotasFiscais { get; set; }
        public DbSet<ItemNotaFiscal> ItensNotaFiscal { get; set; }
        public FaturamentoDbContext(DbContextOptions<FaturamentoDbContext> options) : base(options)
        {
        }
    }
}
