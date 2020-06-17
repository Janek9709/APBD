using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KolosMuzyk.Models
{
    public partial class s18313Context : DbContext
    {
        public s18313Context()
        {
        }

        public s18313Context(DbContextOptions<s18313Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Album> Album { get; set; }
        public virtual DbSet<MusicLabel> MusicLabel { get; set; }
        public virtual DbSet<Musician> Musician { get; set; }
        public virtual DbSet<MusicianTrack> MusicianTrack { get; set; }
        public virtual DbSet<Track> Track { get; set; }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data source=db-mssql;Initial Catalog=s18313;Integrated Security=True");
            }
        }
        */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>(entity =>
            {
                entity.HasKey(e => e.IdAlbum)
                    .HasName("Album_pk");

                entity.Property(e => e.IdAlbum).ValueGeneratedNever();

                entity.Property(e => e.AlbumName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.PublishDate).HasColumnType("datetime");

                entity.HasOne(d => d.IdMusicLabelNavigation)
                    .WithMany(p => p.Album)
                    .HasForeignKey(d => d.IdMusicLabel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Album_MusicLabel");
            });

            modelBuilder.Entity<MusicLabel>(entity =>
            {
                entity.HasKey(e => e.IdMusicLabel)
                    .HasName("MusicLabel_pk");

                entity.Property(e => e.IdMusicLabel).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Musician>(entity =>
            {
                entity.HasKey(e => e.IdMusician)
                    .HasName("Musician_pk");

                entity.Property(e => e.IdMusician).ValueGeneratedNever();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Nickname).HasMaxLength(20);
            });

            modelBuilder.Entity<MusicianTrack>(entity =>
            {
                entity.HasKey(e => e.IdMusicianTrack)
                    .HasName("Musician_Track_pk");

                entity.ToTable("Musician_Track");

                entity.Property(e => e.IdMusicianTrack).ValueGeneratedNever();

                entity.HasOne(d => d.IdMusicianNavigation)
                    .WithMany(p => p.MusicianTrack)
                    .HasForeignKey(d => d.IdMusician)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Musician_Track_Musician");

                entity.HasOne(d => d.IdTrackNavigation)
                    .WithMany(p => p.MusicianTrack)
                    .HasForeignKey(d => d.IdTrack)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Musician_Track_Track");
            });

            modelBuilder.Entity<Track>(entity =>
            {
                entity.HasKey(e => e.IdTrack)
                    .HasName("Track_pk");

                entity.Property(e => e.IdTrack).ValueGeneratedNever();

                entity.Property(e => e.TrackName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.IdMusicAlbumNavigation)
                    .WithMany(p => p.Track)
                    .HasForeignKey(d => d.IdMusicAlbum)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Track_Album");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
