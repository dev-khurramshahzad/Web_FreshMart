using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FreshMart.Models;

public partial class DbFreshMartContext : DbContext
{
    public DbFreshMartContext()
    {
    }

    public DbFreshMartContext(DbContextOptions<DbFreshMartContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=dbFreshMart;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE4E8C94827AD");

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Passwor).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2B3BECD570");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.CategoryStatus)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8BF03FAC7");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.State).HasMaxLength(100);
            entity.Property(e => e.ZipCode).HasMaxLength(10);
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDF6ACDAC235");

            entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");
            entity.Property(e => e.CustomerFid).HasColumnName("CustomerFID");
            entity.Property(e => e.FeedbackDate).HasColumnType("datetime");
            entity.Property(e => e.ItemFid).HasColumnName("ItemFID");

            entity.HasOne(d => d.CustomerF).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CustomerFid)
                .HasConstraintName("FK_Feedbacks_Customers");

            entity.HasOne(d => d.ItemF).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ItemFid)
                .HasConstraintName("FK_Feedbacks_Items");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Items__727E83EB56D21D56");

            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.CategoryFid).HasColumnName("CategoryFID");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Pprice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PPrice");
            entity.Property(e => e.Sprice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("SPrice");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.StockUnit).HasMaxLength(50);

            entity.HasOne(d => d.CategoryF).WithMany(p => p.Items)
                .HasForeignKey(d => d.CategoryFid)
                .HasConstraintName("FK_Items_Categories");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAFFAC93E24");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CustomerFid).HasColumnName("CustomerFID");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.CustomerF).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerFid)
                .HasConstraintName("FK_Orders_Customers");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D30C9160B320");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.ItemFid).HasColumnName("ItemFID");
            entity.Property(e => e.OrderFid).HasColumnName("OrderFID");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.ItemF).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ItemFid)
                .HasConstraintName("FK_OrderDetails_Items");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
