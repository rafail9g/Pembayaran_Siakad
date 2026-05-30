using Microsoft.EntityFrameworkCore;
using Pembayaran_Siakad.Data;
using Pembayaran_Siakad.Models;

namespace Pembayaran_Siakad.Services
{
    public class KeuanganService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<KeuanganService> _logger;

        public KeuanganService(AppDbContext context, ILogger<KeuanganService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Ambil semua data keuangan + info mahasiswa
        public async Task<List<KeuanganResponseDto>> GetAllAsync()
        {
            var data = await _context.Keuangans
                .Include(k => k.Mahasiswa)
                .Include(k => k.RiwayatPembayaran)
                .ToListAsync();

            return data.Select(MapToDto).ToList();
        }

        // Ambil data keuangan berdasarkan ID keuangan
        public async Task<KeuanganResponseDto?> GetByIdAsync(string id)
        {
            var data = await _context.Keuangans
                .Include(k => k.Mahasiswa)
                .Include(k => k.RiwayatPembayaran)
                .FirstOrDefaultAsync(k => k.Id == id);

            return data == null ? null : MapToDto(data);
        }

        // Ambil data keuangan berdasarkan ID mahasiswa
        public async Task<KeuanganResponseDto?> GetByMahasiswaIdAsync(string mahasiswaId)
        {
            var data = await _context.Keuangans
                .Include(k => k.Mahasiswa)
                .Include(k => k.RiwayatPembayaran)
                .FirstOrDefaultAsync(k => k.MahasiswaId == mahasiswaId);

            return data == null ? null : MapToDto(data);
        }

        // Tambah data keuangan baru untuk mahasiswa
        public async Task<KeuanganResponseDto?> TambahKeuanganAsync(TambahKeuanganRequest request)
        {
            // Pastikan mahasiswa ada di database (sudah disync)
            var mahasiswa = await _context.Mahasiswas
                .FirstOrDefaultAsync(m => m.Id == request.MahasiswaId);

            if (mahasiswa == null)
            {
                _logger.LogWarning("Mahasiswa dengan Id {Id} tidak ditemukan", request.MahasiswaId);
                return null;
            }

            // Cek apakah sudah ada data keuangan untuk mahasiswa ini
            var existing = await _context.Keuangans
                .FirstOrDefaultAsync(k => k.MahasiswaId == request.MahasiswaId);

            if (existing != null)
            {
                _logger.LogWarning("Data keuangan untuk mahasiswa {Id} sudah ada", request.MahasiswaId);
                return null;
            }

            var keuangan = new KeuanganEntity
            {
                Id = Guid.NewGuid().ToString(),
                MahasiswaId = request.MahasiswaId,
                NilaiUkt = request.NilaiUkt,
                StatusTagihan = "Belum Lunas",
                TanggalTagihan = DateTime.UtcNow,
                TanggalJatuhTempo = request.TanggalJatuhTempo,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Keuangans.Add(keuangan);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(keuangan.Id);
        }

        // Tambah riwayat pembayaran dan update status tagihan
        public async Task<KeuanganResponseDto?> TambahPembayaranAsync(string keuanganId, TambahPembayaranRequest request)
        {
            var keuangan = await _context.Keuangans
                .Include(k => k.RiwayatPembayaran)
                .FirstOrDefaultAsync(k => k.Id == keuanganId);

            if (keuangan == null)
                return null;

            var pembayaran = new RiwayatPembayaranEntity
            {
                Id = Guid.NewGuid().ToString(),
                KeuanganId = keuanganId,
                JumlahBayar = request.JumlahBayar,
                TanggalBayar = DateTime.UtcNow,
                MetodePembayaran = request.MetodePembayaran,
                Keterangan = request.Keterangan
            };

            _context.RiwayatPembayarans.Add(pembayaran);

            // Hitung total yang sudah dibayar
            var totalBayar = keuangan.RiwayatPembayaran.Sum(r => r.JumlahBayar) + request.JumlahBayar;

            // Update status tagihan otomatis
            if (totalBayar >= keuangan.NilaiUkt)
                keuangan.StatusTagihan = "Lunas";
            else if (totalBayar > 0)
                keuangan.StatusTagihan = "Cicilan";
            else
                keuangan.StatusTagihan = "Belum Lunas";

            keuangan.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(keuanganId);
        }

        private static KeuanganResponseDto MapToDto(KeuanganEntity k)
        {
            return new KeuanganResponseDto
            {
                Id = k.Id,
                MahasiswaId = k.MahasiswaId,
                NamaMahasiswa = k.Mahasiswa?.Nama ?? "",
                ProgramStudi = k.Mahasiswa?.ProgramStudi ?? "",
                StatusAkademik = k.Mahasiswa?.StatusAkademik ?? "",
                NilaiUkt = k.NilaiUkt,
                StatusTagihan = k.StatusTagihan,
                TanggalTagihan = k.TanggalTagihan,
                TanggalJatuhTempo = k.TanggalJatuhTempo,
                RiwayatPembayaran = k.RiwayatPembayaran.Select(r => new RiwayatPembayaranDto
                {
                    Id = r.Id,
                    JumlahBayar = r.JumlahBayar,
                    TanggalBayar = r.TanggalBayar,
                    MetodePembayaran = r.MetodePembayaran,
                    Keterangan = r.Keterangan
                }).ToList()
            };
        }
    }
}
