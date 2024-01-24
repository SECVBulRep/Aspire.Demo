using Microsoft.EntityFrameworkCore;

namespace Web.Db;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        
        Database.EnsureDeleted();   // удаляем бд со старой схемой
        Database.EnsureCreated();   // создаем бд с новой схемой
    }
}