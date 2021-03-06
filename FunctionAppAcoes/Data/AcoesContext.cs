using Microsoft.EntityFrameworkCore;
using FunctionAppAcoes.Models;

namespace FunctionAppAcoes.Data
{
    public class AcoesContext : DbContext
    {
        public DbSet<Acao> Acoes { get; set; }

        public AcoesContext(DbContextOptions<AcoesContext> options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Acao>()
                .HasKey(c => c.Id);
        }
    }
}