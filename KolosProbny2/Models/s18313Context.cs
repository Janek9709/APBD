using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KolosProbny2.Models
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

        public virtual DbSet<Klient> Klient { get; set; }
        public virtual DbSet<Pracownik> Pracownik { get; set; }
        public virtual DbSet<WyrobCukierniczy> WyrobCukierniczy { get; set; }
        public virtual DbSet<Zamowienie> Zamowienie { get; set; }
        public virtual DbSet<ZamowienieWyrobCukierniczy> ZamowienieWyrobCukierniczy { get; set; }

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
            modelBuilder.Entity<Klient>(entity =>
            {
                entity.HasKey(e => e.IdKlient)
                    .HasName("Klient_pk");

                entity.Property(e => e.IdKlient).ValueGeneratedNever();

                entity.Property(e => e.Imie)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Nazwisko)
                    .IsRequired()
                    .HasMaxLength(60);
            });

            modelBuilder.Entity<Pracownik>(entity =>
            {
                entity.HasKey(e => e.IdPracownik)
                    .HasName("Pracownik_pk");

                entity.Property(e => e.IdPracownik).ValueGeneratedNever();

                entity.Property(e => e.Imie)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Nazwisko)
                    .IsRequired()
                    .HasMaxLength(60);
            });

            modelBuilder.Entity<WyrobCukierniczy>(entity =>
            {
                entity.HasKey(e => e.IdWyrobuCukierniczego)
                    .HasName("WyrobCukierniczy_pk");

                entity.Property(e => e.IdWyrobuCukierniczego).ValueGeneratedNever();

                entity.Property(e => e.Nazwa)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Typ)
                    .IsRequired()
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<Zamowienie>(entity =>
            {
                entity.HasKey(e => e.IdZamowienia)
                    .HasName("Zamowienie_pk");

                entity.Property(e => e.IdZamowienia).ValueGeneratedNever();

                entity.Property(e => e.DataPrzyjecia).HasColumnType("datetime");

                entity.Property(e => e.DataRealizacji).HasColumnType("datetime");

                entity.Property(e => e.KlientIdKlient).HasColumnName("Klient_IdKlient");

                entity.Property(e => e.PracownikIdPracownik).HasColumnName("Pracownik_IdPracownik");

                entity.Property(e => e.Uwagi).HasMaxLength(30);

                entity.HasOne(d => d.KlientIdKlientNavigation)
                    .WithMany(p => p.Zamowienie)
                    .HasForeignKey(d => d.KlientIdKlient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Zamowienie_Klient");

                entity.HasOne(d => d.PracownikIdPracownikNavigation)
                    .WithMany(p => p.Zamowienie)
                    .HasForeignKey(d => d.PracownikIdPracownik)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Zamowienie_Pracownik");
            });

            modelBuilder.Entity<ZamowienieWyrobCukierniczy>(entity =>
            {
                entity.HasKey(e => new { e.IdWyrobuCukierniczego, e.IdZamowienia })
                    .HasName("Zamowienie_WyrobCukierniczy_pk");

                entity.ToTable("Zamowienie_WyrobCukierniczy");

                entity.Property(e => e.Uwagi).HasMaxLength(300);

                entity.HasOne(d => d.IdWyrobuCukierniczegoNavigation)
                    .WithMany(p => p.ZamowienieWyrobCukierniczy)
                    .HasForeignKey(d => d.IdWyrobuCukierniczego)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Zamowienie_WyrobCukierniczy_WyrobCukierniczy");

                entity.HasOne(d => d.IdZamowieniaNavigation)
                    .WithMany(p => p.ZamowienieWyrobCukierniczy)
                    .HasForeignKey(d => d.IdZamowienia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Zamowienie_WyrobCukierniczy_Zamowienie");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
