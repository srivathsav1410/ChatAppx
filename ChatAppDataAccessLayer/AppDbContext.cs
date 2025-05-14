using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatAppDataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAppDataAccessLayer
{
    public class ChatAppDbContext : DbContext
    {
        public ChatAppDbContext()
        {
        }

        public ChatAppDbContext(DbContextOptions<ChatAppDbContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Friend> Friends { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog=ChatAppCore;Integrated Security=true");
            }

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Friend → SentUser and RequestUser
            modelBuilder.Entity<Friend>()
                .HasOne(f => f.SentUser)
                .WithMany(u => u.FriendRequestsSent)
                .HasForeignKey(f => f.SentUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.RequestUser)
                .WithMany(u => u.FriendRequestsReceived)
                .HasForeignKey(f => f.RequestUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Message → Sender
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.MessagesSent)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Message → Receiver
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.MessagesReceived)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>()
      .HasIndex(u => u.Email)
      .IsUnique();
            modelBuilder.Entity<User>()
     .HasIndex(u => u.Username)
     .IsUnique();

        }



    }

}
