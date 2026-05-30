using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Pembayaran_Siakad.Data;
using Pembayaran_Siakad.Models;

namespace Pembayaran_Siakad.Services
{
    public class SyncService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SyncService> _logger;

        public SyncService(
            HttpClient httpClient,
            AppDbContext context,
            IConfiguration configuration,
            ILogger<SyncService> logger)
        {
            _httpClient = httpClient;
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<(int added, int updated)> SyncMahasiswaAsync()
        {
            var baseUrl = _configuration["MahasiswaApi:BaseUrl"]
                ?? "https://mahasiswa-api-psi.vercel.app";

            _logger.LogInformation("Memulai sinkronisasi dari {BaseUrl}", baseUrl);

            var response = await _httpClient.GetAsync($"{baseUrl}/api/mahasiswa");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<MahasiswaApiResponse>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (apiResponse == null || !apiResponse.Success)
            {
                _logger.LogWarning("Response API tidak valid atau success=false");
                return (0, 0);
            }

            int added = 0, updated = 0;

            foreach (var item in apiResponse.Data)
            {
                var existing = await _context.Mahasiswas
                    .FirstOrDefaultAsync(m => m.Id == item._id);

                if (existing == null)
                {
                    // Insert baru
                    var newMahasiswa = new MahasiswaEntity
                    {
                        Id = item._id,
                        Nama = item.Nama,
                        ProgramStudi = item.ProgramStudi,
                        StatusAkademik = item.StatusAkademik,
                        MataKuliah = item.MataKuliah,
                        CreatedAt = item.CreatedAt
                    };
                    _context.Mahasiswas.Add(newMahasiswa);
                    added++;

                    _logger.LogInformation("Mahasiswa baru ditambahkan: {Nama} ({Id})", item.Nama, item._id);
                }
                else
                {
                    // Update data yang sudah ada
                    existing.Nama = item.Nama;
                    existing.ProgramStudi = item.ProgramStudi;
                    existing.StatusAkademik = item.StatusAkademik;
                    existing.MataKuliah = item.MataKuliah;
                    updated++;

                    _logger.LogInformation("Mahasiswa diperbarui: {Nama} ({Id})", item.Nama, item._id);
                }
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Sinkronisasi selesai. Ditambahkan: {Added}, Diperbarui: {Updated}", added, updated);
            return (added, updated);
        }
    }
}
