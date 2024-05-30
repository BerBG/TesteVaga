using Microsoft.EntityFrameworkCore;

namespace APITeste.Models
{
    public class EFDBContext : DbContext
    {
        public EFDBContext(DbContextOptions<EFDBContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
    }
}
