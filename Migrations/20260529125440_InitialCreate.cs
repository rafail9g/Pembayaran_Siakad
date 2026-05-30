using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pembayaran_Siakad.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mahasiswas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Nama = table.Column<string>(type: "text", nullable: false),
                    ProgramStudi = table.Column<string>(type: "text", nullable: false),
                    StatusAkademik = table.Column<string>(type: "text", nullable: false),
                    MataKuliahJson = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mahasiswas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Keuangans",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    MahasiswaId = table.Column<string>(type: "text", nullable: false),
                    NilaiUkt = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    StatusTagihan = table.Column<string>(type: "text", nullable: false),
                    TanggalTagihan = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TanggalJatuhTempo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keuangans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Keuangans_Mahasiswas_MahasiswaId",
                        column: x => x.MahasiswaId,
                        principalTable: "Mahasiswas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiwayatPembayarans",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    KeuanganId = table.Column<string>(type: "text", nullable: false),
                    JumlahBayar = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TanggalBayar = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MetodePembayaran = table.Column<string>(type: "text", nullable: false),
                    Keterangan = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiwayatPembayarans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiwayatPembayarans_Keuangans_KeuanganId",
                        column: x => x.KeuanganId,
                        principalTable: "Keuangans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Keuangans_MahasiswaId",
                table: "Keuangans",
                column: "MahasiswaId");

            migrationBuilder.CreateIndex(
                name: "IX_RiwayatPembayarans_KeuanganId",
                table: "RiwayatPembayarans",
                column: "KeuanganId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiwayatPembayarans");

            migrationBuilder.DropTable(
                name: "Keuangans");

            migrationBuilder.DropTable(
                name: "Mahasiswas");
        }
    }
}
