using JustTradeIt.Software.API.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace JustTradeIt.Software.API.Repositories
{
    public class JustTradeItContext : DbContext
    {
        public JustTradeItContext(DbContextOptions<JustTradeItContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; } = null!;
        public DbSet<Trade> Trade { get; set; } = null!;
        public DbSet<TradeItem> TradeItem { get; set; } = null!;
        public DbSet<ItemCondition> ItemCondition { get; set; } = null!;
        public DbSet<Item> Item { get; set; } = null!;
        public DbSet<ItemImage> ItemImage { get; set; } = null!;
        public DbSet<JwtToken> JwtToken { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=JustTradeIt_DB.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Item>().HasOne(i => i.Owner).WithMany(u => u.relatedItems).HasForeignKey(i => i.ownerId);

            modelBuilder.Entity<Trade>().HasOne(t => t.Sender).WithMany(u => u.SentTrades).HasForeignKey(t => t.SenderId);

            modelBuilder.Entity<Trade>().HasOne(t => t.Receiver).WithMany(u => u.ReceivedTrades).HasForeignKey(t => t.ReceiverId);
            modelBuilder.Entity<TradeItem>().HasKey(ti => new { ti.ItemId, ti.TradeId });

            modelBuilder.Entity<TradeItem>()
           .HasOne(ti => ti.item)
           .WithMany(i => i.relatedTradeItems)
           .HasForeignKey(ti => ti.ItemId);

            modelBuilder.Entity<TradeItem>()
                .HasOne(ti => ti.trade)
                .WithMany(t => t.RelatedtradeItems)
                .HasForeignKey(ti => ti.TradeId);

            modelBuilder.Entity<TradeItem>()
               .HasOne(ti => ti.user)
               .WithMany(u => u.RelatedTradeItems)
               .HasForeignKey(ti => ti.UserId);
        }
    }
}