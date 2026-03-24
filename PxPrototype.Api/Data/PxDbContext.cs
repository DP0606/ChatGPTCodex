using Microsoft.EntityFrameworkCore;
using PxPrototype.Api.Models;

namespace PxPrototype.Api.Data;

public class PxDbContext(DbContextOptions<PxDbContext> options) : DbContext(options)
{
    public DbSet<PxDataset> Datasets => Set<PxDataset>();
    public DbSet<PxVariable> Variables => Set<PxVariable>();
    public DbSet<PxVariableValue> VariableValues => Set<PxVariableValue>();
    public DbSet<PxObservation> Observations => Set<PxObservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PxDataset>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.FileName).HasMaxLength(260);
            entity.Property(x => x.Title).HasMaxLength(1024);
        });

        modelBuilder.Entity<PxVariable>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Code).HasMaxLength(128);
            entity.Property(x => x.Name).HasMaxLength(512);
            entity.HasOne(x => x.PxDataset)
                .WithMany(x => x.Variables)
                .HasForeignKey(x => x.PxDatasetId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PxVariableValue>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Code).HasMaxLength(128);
            entity.Property(x => x.Label).HasMaxLength(512);
            entity.HasOne(x => x.PxVariable)
                .WithMany(x => x.Values)
                .HasForeignKey(x => x.PxVariableId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PxObservation>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.CoordinateJson).HasColumnType("TEXT");
            entity.Property(x => x.Status).HasMaxLength(32);
            entity.HasOne(x => x.PxDataset)
                .WithMany(x => x.Observations)
                .HasForeignKey(x => x.PxDatasetId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
