using System;
using Microsoft.EntityFrameworkCore;
using Ciomag_Andreea_Museum.Models;

namespace Ciomag_Andreea_Museum.Data
{
    public class MuseumContext : DbContext
    {
        public MuseumContext(DbContextOptions<MuseumContext> options) : base(options)
        {
        }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Exhibit> Exhibits { get; set; }
        public DbSet<Exhibition> Exhibitions { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Visit> Visits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>().ToTable("Artist");
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<Exhibit>().ToTable("Exhibit");
            modelBuilder.Entity<Exhibition>().ToTable("Exhibition");
            modelBuilder.Entity<Gallery>().ToTable("Gallery");
            modelBuilder.Entity<Visit>().ToTable("Visit");
        }
    }
}

