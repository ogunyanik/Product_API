using Microsoft.EntityFrameworkCore;
using Product_API.Core.Models;

namespace Product_API.Infrastructure.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
 

}