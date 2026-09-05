using System;
using System.Collections.Generic;
using System.Linq;
using EstoqueService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;


namespace EstoqueService.Data
{
    public class EstoqueDbContext : DbContext
    {
        public DbSet<Produto> Produtos { get; set; }
        public EstoqueDbContext(DbContextOptions<EstoqueDbContext> options) : base(options)
        {
        }
    }
}
