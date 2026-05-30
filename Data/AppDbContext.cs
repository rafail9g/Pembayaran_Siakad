using Microsoft.EntityFrameworkCore;
using Pembayaran_Siakad.Models;

namespace Pembayaran_Siakad.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<MahasiswaEntity> Mahasiswas => Set<MahasiswaEntity>();

        public DbSet<KeuanganEntity> Keuangans => Set<KeuanganEntity>();

        public DbSet<RiwayatPembayaranEntity> RiwayatPembayarans => Set<RiwayatPembayaranEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<KeuanganEntity>()
                .HasOne(k => k.Mahasiswa)
                .WithMany()
                .HasForeignKey(k => k.MahasiswaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RiwayatPembayaranEntity>()
                .HasOne(r => r.Keuangan)
                .WithMany(k => k.RiwayatPembayaran)
                .HasForeignKey(r => r.KeuanganId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KeuanganEntity>()
                .Property(k => k.NilaiUkt)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<RiwayatPembayaranEntity>()
                .Property(r => r.JumlahBayar)
                .HasColumnType("decimal(18,2)");
        }
    }
}
