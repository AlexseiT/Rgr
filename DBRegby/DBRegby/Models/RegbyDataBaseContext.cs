using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DBRegby.Models
{
    public partial class RegbyDataBaseContext : DbContext
    {
        public RegbyDataBaseContext()
        {
        }

        public RegbyDataBaseContext(DbContextOptions<RegbyDataBaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Competition> Competitions { get; set; } = null!;
        public virtual DbSet<CompetitionTeam> CompetitionTeams { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<Player> Players { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=G:\\repos\\DBRegby\\DBRegby\\Assets\\RegbyDataBase.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Competition>(entity =>
            {
                entity.HasKey(e => e.Competition1);

                entity.ToTable("Competition");

                entity.Property(e => e.Competition1).HasColumnName("Competition");

                entity.Property(e => e.EndDate)
                    .HasColumnType("DATE")
                    .HasColumnName("End Date");

                entity.Property(e => e.StartDate)
                    .HasColumnType("DATE")
                    .HasColumnName("Start Date");
            });

            modelBuilder.Entity<CompetitionTeam>(entity =>
            {
                entity.HasKey(e => new { e.Competition, e.Team });

                entity.ToTable("CompetitionTeam");

                entity.HasOne(d => d.CompetitionNavigation)
                    .WithMany(p => p.CompetitionTeams)
                    .HasForeignKey(d => d.Competition)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TeamNavigation)
                    .WithMany(p => p.CompetitionTeams)
                    .HasForeignKey(d => d.Team)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Game");

                entity.Property(e => e.Id)
                    .HasColumnType("DOUBLE")
                    .HasColumnName("id");

                entity.HasOne(d => d.CompetitionNavigation)
                    .WithMany(p => p.GamesNavigation)
                    .HasForeignKey(d => d.Competition);
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("Player");

                entity.Property(e => e.Id)
                    .HasColumnType("DOUBLE")
                    .HasColumnName("id");

                entity.Property(e => e.DropGoals).HasColumnName("Drop Goals");

                entity.Property(e => e.Player1).HasColumnName("Player");

                entity.Property(e => e.PointsFor).HasColumnName("Points For");

                entity.Property(e => e.TestCaps)
                    .HasColumnName("Test Caps")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.TeamNavigation)
                    .WithMany(p => p.Players)
                    .HasForeignKey(d => d.Team);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.Team1);

                entity.ToTable("Team");

                entity.Property(e => e.Team1).HasColumnName("Team");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
