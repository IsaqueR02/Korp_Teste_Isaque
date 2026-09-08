using FaturamentoService.Data;
using FaturamentoService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

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
        [HttpPost]
        [Route("api/notasfiscais/{numero}/imprimir")]
        public async Task<IActionResult> ImpressNotaFiscal([FromServices] IHttpClientFactory httpClientFactory, [FromServices] FaturamentoDbContext context, [FromRoute] int numero)
        {
            var client = httpClientFactory.CreateClient();
            var notaFiscal = await context.NotasFiscais.Include(n => n.Itens).FirstOrDefaultAsync(n => n.NumeroSequencial == numero);
            if (notaFiscal == null)
            {
                return NotFound(new { message = $"Nota Fiscal não encontrada." });
            }
            var statusFechada = NotaFiscal.statusNotaFiscal.Fechada;
            try
            {
                foreach (var item in notaFiscal.Itens)
                {
                    var response = await client.PostAsync("http://localhost:5001/api/produtos/reduzir-saldo", JsonContent.Create(new { codigo = item.ProdutoCodigo, quantidade = item.Quantidade }));
                    if (response.IsSuccessStatusCode)
                    {
                        return Ok(new { message = $"Nota Fiscal {notaFiscal.NumeroSequencial} impressa com sucesso." });
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        return BadRequest(new { message = $"Erro ao reduzir saldo do produto {item.ProdutoCodigo}: {errorMessage}" });
                    }
                    else
                    {
                        return StatusCode(503, new { message = $"Erro ao reduzir saldo do produto {item.ProdutoCodigo}. Tente novamente mais tarde." });
                    }
                }
            } catch (HttpRequestException) 
            {
                return StatusCode(503, new { message = $"Erro no fechamento da Nota Fiscal {notaFiscal.NumeroSequencial}. Não se preocupe, a Nota Fiscal será impressa assim que o problema for resolvido." });
            }
            if (notaFiscal.Status == statusFechada)
            {
                return BadRequest(new { message = $"Nota Fiscal {notaFiscal.NumeroSequencial} está fechada." });
            }
            await context.SaveChangesAsync();
            // Simulate printing the invoice
            // In a real application, you would send the invoice to a printer or generate a PDF
            Console.WriteLine($"Imprimindo Nota Fiscal {notaFiscal.NumeroSequencial} com {notaFiscal.Itens.Count} items.");
            return Ok(new { message = $"Nota Fiscal {notaFiscal.NumeroSequencial} impressa com sucesso." });
        }
    }
}
