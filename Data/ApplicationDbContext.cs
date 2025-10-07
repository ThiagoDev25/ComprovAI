using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ComprovAI.Models;

namespace ComprovAI.Data;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<PaymentModeModel> Payments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurações para o provedor Cosmos DB
        modelBuilder.Entity<PaymentModeModel>()
            .ToContainer("Payments") // <-- FORÇA O NOME DO CONTAINER
            .HasPartitionKey(p => p.Type) // Particiona por Tipo de Pagamento (Débito/Crédito/Pix)
            .Property(p => p.Id)
            .ToJsonProperty("id");
        // No Cosmos DB, a chave primária deve ser mapeada para a propriedade 'id'
        // Embora seu modelo tenha 'Id', o EF Core para Cosmos DB cuida do mapeamento.
        // O Id deve ser string para Cosmos DB, mas o EF Core pode gerenciar isso.
        
        // Se você quiser que o campo Id seja a chave primária do Cosmos DB,
        // o tipo ideal para a propriedade Id no modelo é string.
        // Se mantiver 'int', o EF Core fará o mapeamento.
        
        // Se o seu modelo for: public int Id { get; set; }, é melhor mudar para:
        // public string Id { get; set; } = Guid.NewGuid().ToString();
        
        base.OnModelCreating(modelBuilder);
    }

}