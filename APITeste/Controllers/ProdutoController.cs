using APITeste.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITeste.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly EFDBContext _dbContext;

        public ProdutoController(EFDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            return await _dbContext.Produtos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> Get(int id)
        {
            var produto = await _dbContext.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        [HttpPost]
        public ActionResult<Produto> Post([FromBody] ProdutoInputDto produtoInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var produto = new Produto
            {
                Nome = produtoInput.Nome,
                Descricao = produtoInput.Descricao,
                Quantidade = produtoInput.Quantidade,
                Valor = produtoInput.Valor,
                Categoria = produtoInput.Categoria
            };

            _dbContext.Produtos.Add(produto);
            _dbContext.SaveChanges();

            return Ok(produto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProdutoInputDto produtoInput)
        {
            var produto = await _dbContext.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            produto.Nome = produtoInput.Nome;
            produto.Descricao = produtoInput.Descricao;
            produto.Quantidade = produtoInput.Quantidade;
            produto.Valor = produtoInput.Valor;
            produto.Categoria = produtoInput.Categoria;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var produto = await _dbContext.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            _dbContext.Produtos.Remove(produto);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdutoExists(int id)
        {
            return _dbContext.Produtos.Any(e => e.Id == id);
        }
    }
}
