using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BloodBank.Api.Models;

public partial class BloodBankContext : DbContext
{
    public BloodBankContext()
    {
    }

    public BloodBankContext(DbContextOptions<BloodBankContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BloodDonation> BloodDonations { get; set; }

    public virtual DbSet<BloodProduct> BloodProducts { get; set; }

    public virtual DbSet<Donor> Donors { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LIN-5CD5334YCF;Database=BBMS;User Id=sa;Password=Password@123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BloodDonation>(entity =>
        {
            entity.HasKey(e => e.DonationId).HasName("PK__BloodDon__C5082EFBE7F45E36");

            entity.Property(e => e.DonationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PlateletsCount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Rbccount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("RBCCount");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VolumeInMl).HasColumnName("VolumeInML");
            entity.Property(e => e.Wbccount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("WBCCount");

            entity.HasOne(d => d.Donor).WithMany(p => p.BloodDonations)
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BloodDonations_Donors");
        });

        modelBuilder.Entity<BloodProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__BloodPro__B40CC6CD4C124EF2");

            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Donor>(entity =>
        {
            entity.HasKey(e => e.DonorId).HasName("PK__Donors__052E3F7858313428");

            entity.HasIndex(e => e.UserId, "UQ__Donors__1788CC4D497A3C91").IsUnique();

            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.BloodGroup)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithOne(p => p.Donor)
                .HasForeignKey<Donor>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Donors_Users");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PK__Inventor__F5FDE6B376C8305E");

            entity.ToTable("Inventory");

            entity.Property(e => e.BloodGroup)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Products");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E123D8CFB55");

            entity.Property(e => e.Message).IsUnicode(false);
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notifications_Users");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCFE78C5425");

            entity.Property(e => e.BloodGroup)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeliveryAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.DeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Products");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Users");
        });

        modelBuilder.Entity<OrderStatusHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__OrderSta__4D7B4ABDD215EF8A");

            entity.ToTable("OrderStatusHistory");

            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderStatusHistories)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusHistory_Orders");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.OrderStatusHistories)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusHistory_Users");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A3882335E56");

            entity.Property(e => e.AmountPaid).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentGateway)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransactionId)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Orders");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1A5D2AF440");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B616082CADD61").IsUnique();

            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CB272885D");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105345457345A").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash).IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
