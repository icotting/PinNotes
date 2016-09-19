using Microsoft.EntityFrameworkCore;
using PinNotes.Accessors.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PinNotes.Accessors.Domain.Core
{
    public class PersistenceContext : DbContext
    {
        public PersistenceContext(DbContextOptions options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Pin> Pins { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany<Pin>(u => u.Pins)
                .WithOne(p => p.User)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade);

            modelBuilder.Entity<Pin>()
                .HasMany<Note>(p => p.Notes)
                .WithOne(n => n.Pin)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade);
        }
    }
}
