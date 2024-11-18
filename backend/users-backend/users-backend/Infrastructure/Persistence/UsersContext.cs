using users_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace users_backend.Infrastructure.Persistence;

public class UsersContext : DbContext
{
    public UsersContext(DbContextOptions<UsersContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder _optionsBuilder)
    {
        base.OnConfiguring(_optionsBuilder);
    }

    // No debería dejarnos dar de alta un usuario con un RolId que no existe
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(i => i.Role)
            .WithMany()
            .HasForeignKey(i => i.RoleId)
            .IsRequired();
        modelBuilder.Entity<User>()
            .Property(u => u.RowVersion)
            .IsRowVersion();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
}