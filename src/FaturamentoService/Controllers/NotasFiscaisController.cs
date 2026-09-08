using FaturamentoService.Data;
using FaturamentoService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FaturamentoService.Controllers
{
    public class NotasFiscaisController : ControllerBase
    {
        [HttpGet]
        [Route("api/notasfiscais")]
        public IActionResult GetNotasFiscais([FromServices] FaturamentoDbContext context)
        {
            var notasFiscais = context.NotasFiscais.Include(n => n.Itens).ToList();
            return Ok(notasFiscais);
        }
        [HttpPost]
        [Route("api/notasfiscais")]
        public IActionResult CreateNotaFiscal([FromServices] FaturamentoDbContext context, [FromBody] NotaFiscal notaFiscal)
        {
            context.NotasFiscais.Add(notaFiscal);
            context.SaveChanges();
            return CreatedAtAction(nameof(GetNotasFiscais), new { id = notaFiscal.NumeroSequencial }, notaFiscal);
        }
    }
}
