using System;
using Microsoft.EntityFrameworkCore;
using PaintStore.Models;

namespace PaintStore.API.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }

    public DbSet<PaintProduct> PaintProducts {get;set;} //声明数据库里面需要这个table


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PaintProduct>()
        .Property(p=>p.Price)
        .HasPrecision(18, 2); 
        modelBuilder.Entity<PaintProduct>()
        .HasKey(p => p.ProductId);
        modelBuilder.Entity<PaintProduct>()
        .OwnsOne(p => p.Brand);
        modelBuilder.Entity<PaintProduct>()
        .OwnsOne(p => p.Specification);
    }
}
