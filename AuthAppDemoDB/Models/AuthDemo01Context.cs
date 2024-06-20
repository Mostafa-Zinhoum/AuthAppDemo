using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AuthAppDemoDB.Models;

public partial class AuthDemo01Context : DbContext
{
    public AuthDemo01Context()
    {
    }

    public AuthDemo01Context(DbContextOptions<AuthDemo01Context> options)
        : base(options)
    {
    }

    public virtual DbSet<UserInfo> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-4V3EIU4\\SQLEX2019;Database=AuthDemo01;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.Property(e => e.ConfirmationCode).HasMaxLength(50);
            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.DeleteTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsEmailAddressConfirmed).HasDefaultValue(false);
            entity.Property(e => e.IsPhoneNumberConfirmed).HasDefaultValue(false);
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
