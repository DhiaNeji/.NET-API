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
            modelBuilder.Entity<TradeItem>().HasKey(c => new { c.itemId,c.tradeId,c.userID});
            modelBuilder.Entity<Trade>()
         .HasOne(t => t.Sender)
         .WithMany(b => b.userTrade)
         .HasForeignKey(t=>t.SenderId);
            modelBuilder.Entity<Trade>()
         .HasOne(t => t.Receiver)
         .WithMany(b => b.userTrades)
         .HasForeignKey(t => t.ReceiverId);
        }
    }
}