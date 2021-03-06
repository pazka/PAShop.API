﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace Model
{
    public class PAShopDbContext : DbContext
    {
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<Basket> Baskets { get; set; }
        public virtual DbSet<StockMovement> StockMovements { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public PAShopDbContext()
        {
        }

        public PAShopDbContext(DbContextOptions<PAShopDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>().HasKey(x => x.Id);
            modelBuilder.Entity<Inventory>().HasKey(x => x.Id);
            modelBuilder.Entity<Basket>().HasKey(x => x.Id);
            modelBuilder.Entity<StockMovement>().HasKey(x => x.Id);
            modelBuilder.Entity<Transaction>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().HasKey(x => x.Id);


            modelBuilder.Entity<User>()
                .HasMany<Basket>(u => u.Baskets)
                .WithOne(b => b.Owner);

            modelBuilder.Entity<User>()
                .HasMany<Transaction>(u => u.Payments)
                .WithOne(t => t.Owner)
                .HasForeignKey(t => t.OwnerId);

            modelBuilder.Entity<Basket>()
                .HasOne<Transaction>(b => b.Transaction)
                .WithOne(t => t.Order)
                .HasForeignKey<Transaction>( t => t.OrderId);

            modelBuilder.Entity<Basket>()
                .HasMany<BasketItem>(b => b.BasketItems)
                .WithOne(bi => bi.Basket)
                .HasForeignKey(bi => bi.BasketId);

            modelBuilder.Entity<Item>()
                .HasMany<BasketItem>(b => b.BasketItems)
                .WithOne(bi => bi.Item)
                .HasForeignKey(bi => bi.ItemId);

            modelBuilder.Entity<BasketItem>()
                .HasKey(bi => new { bi.BasketId, bi.ItemId});

            modelBuilder.Entity<Item>()
                .HasMany<StockMovement>(i => i.StockMovements)
                .WithOne(sm => sm.Item);

            modelBuilder.Entity<Item>()
                .HasMany<Inventory>(i => i.Inventories)
                .WithOne(i => i.Item);

            modelBuilder.Entity<StockMovement>()
                .HasOne<Inventory>(i => i.LastInventory)
                .WithMany(i => i.StockMovements);

            modelBuilder.Entity<User>().Ignore(p => p.Token);
            modelBuilder.Entity<User>()
                .HasIndex(p => new {p.Email}).IsUnique();
        }
    }
}
