using Domain.Models;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.DataBase.Context;

public class ServerDbContext : DbContext
{
    public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(user => user.Urls)
            .WithOne(url => url.User)
            .HasForeignKey(url => url.UserId)
            .IsRequired(false);
    }
}
