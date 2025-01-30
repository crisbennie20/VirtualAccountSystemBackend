using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VirtualAccountSystemBackend.DTO;

namespace VirtualAccountSystemBackend.Model;

public partial class VASContext : DbContext
{
    public VASContext()
    {
    }

    public VASContext(DbContextOptions<VASContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
        var configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

    }

    public virtual DbSet<ApiAccessInfo> ApiAccessInfos { get; set; }

    public virtual DbSet<CustomerAccountInfo> CustomerAccountInfos { get; set; }

    public virtual DbSet<CustomerProfile> CustomerProfiles { get; set; }

    public virtual DbSet<CustomerTransaction> CustomerTransactions { get; set; }
    public virtual DbSet<AllVirtualAccountDTO> AllVirtualAccountDTOs { get; set; }
    public virtual DbSet<CustomerTransaction> CustomerTransactions1 { get; set; }
    public virtual DbSet<TransactionData> TransactionDatas { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerAccountInfo>(entity =>
        {
            entity.Property(e => e.VirtualAccountNo).IsFixedLength();
        });

        modelBuilder.Entity<AllVirtualAccountDTO>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<TransactionData>(entity =>
        {
            entity.HasNoKey();
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
