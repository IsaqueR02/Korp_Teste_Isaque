using EstoqueService.Data;
using EstoqueService.Models;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueService.Controllers
{
    public class ProdutosController : ControllerBase
    {
        [HttpGet]
        [Route("api/produtos")]
        public IActionResult GetProdutos([FromServices] EstoqueDbContext context)
        {
            var produtos = context.Produtos.ToList();
            return Ok(produtos);
        }
        [HttpPost]
        [Route("api/produtos")]
        public IActionResult CreateProduto([FromServices] EstoqueDbContext context, [FromBody] Produto produto)
        {
            context.Produtos.Add(produto);
            context.SaveChanges();
            return CreatedAtAction(nameof(GetProdutos), new { id = produto.Codigo }, produto);
        }
        [HttpPost]
        [Route("api/produtos/reduzir-saldo")]
        public IActionResult ReduzirSaldo([FromServices] EstoqueDbContext context, [FromBody] Produto produto)
        {
            int codigo = produto.Codigo;
            var produtoBanco = context.Produtos.FirstOrDefault(p => p.Codigo == codigo);
            decimal saldo = produto.Saldo;
            decimal quantidadeSolicitada = produto.Saldo; // Assuming the requested quantity is passed in the Saldo property for this example
            if (produtoBanco == null)
            {
                return NotFound(new { message = $"Produto com código {codigo} não encontrado." });
            }
            if (produtoBanco.Saldo < quantidadeSolicitada)
            {
                return BadRequest(new { message = $"Saldo insuficiente para o produto {codigo}. Saldo atual: {produto.Saldo}, quantidade solicitada: {quantidadeSolicitada}." });
            }
            produtoBanco.Saldo -= quantidadeSolicitada;
            context.SaveChanges();
            return Ok(new { message = $"Saldo do produto {codigo} reduzido em {quantidadeSolicitada}. Saldo atual: {produto.Saldo}." });
        }
    }
}
