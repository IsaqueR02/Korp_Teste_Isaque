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
    }
}
