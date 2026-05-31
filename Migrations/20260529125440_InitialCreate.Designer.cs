using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pembayaran_Siakad.Data;

namespace Pembayaran_Siakad.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260529125440_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Pembayaran_Siakad.Models.KeuanganEntity", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("text");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp with time zone");

                b.Property<string>("MahasiswaId")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<decimal>("NilaiUkt")
                    .HasColumnType("decimal(18,2)");

                b.Property<string>("StatusTagihan")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<DateTime?>("TanggalJatuhTempo")
                    .HasColumnType("timestamp with time zone");

                b.Property<DateTime>("TanggalTagihan")
                    .HasColumnType("timestamp with time zone");

                b.Property<DateTime>("UpdatedAt")
                    .HasColumnType("timestamp with time zone");

                b.HasKey("Id");

                b.HasIndex("MahasiswaId");

                b.ToTable("Keuangans");
            });

            modelBuilder.Entity("Pembayaran_Siakad.Models.MahasiswaEntity", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("text");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp with time zone");

                b.Property<string>("MataKuliahJson")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<string>("Nama")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<string>("ProgramStudi")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<string>("StatusAkademik")
                    .IsRequired()
                    .HasColumnType("text");

                b.HasKey("Id");

                b.ToTable("Mahasiswas");
            });

            modelBuilder.Entity("Pembayaran_Siakad.Models.RiwayatPembayaranEntity", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("text");

                b.Property<decimal>("JumlahBayar")
                    .HasColumnType("decimal(18,2)");

                b.Property<string>("Keterangan")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<string>("KeuanganId")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<string>("MetodePembayaran")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<DateTime>("TanggalBayar")
                    .HasColumnType("timestamp with time zone");

                b.HasKey("Id");

                b.HasIndex("KeuanganId");

                b.ToTable("RiwayatPembayarans");
            });

            modelBuilder.Entity("Pembayaran_Siakad.Models.KeuanganEntity", b =>
            {
                b.HasOne("Pembayaran_Siakad.Models.MahasiswaEntity", "Mahasiswa")
                    .WithMany()
                    .HasForeignKey("MahasiswaId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Mahasiswa");
            });

            modelBuilder.Entity("Pembayaran_Siakad.Models.RiwayatPembayaranEntity", b =>
            {
                b.HasOne("Pembayaran_Siakad.Models.KeuanganEntity", "Keuangan")
                    .WithMany("RiwayatPembayaran")
                    .HasForeignKey("KeuanganId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Keuangan");
            });

            modelBuilder.Entity("Pembayaran_Siakad.Models.KeuanganEntity", b =>
            {
                b.Navigation("RiwayatPembayaran");
            });
#pragma warning restore 612, 618
        }
    }
}