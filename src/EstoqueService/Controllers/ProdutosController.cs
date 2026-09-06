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
        public IActionResult ReduzirSaldo([FromServices] EstoqueDbContext context, [FromBody] Produto request)
        {
            var produto = context.Produtos.FirstOrDefault(p => p.Codigo == request.Codigo);
            if (produto == null)
            {
                return NotFound();
            }
            if (produto.Saldo < request.Saldo)
            {
                return BadRequest("Saldo insuficiente.");
            }
            produto.Saldo -= request.Saldo;
            context.SaveChanges();
            return Ok(produto);
        }
    }
}
