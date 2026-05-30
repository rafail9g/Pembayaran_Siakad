using Microsoft.AspNetCore.Mvc;
using Pembayaran_Siakad.Services;

namespace Pembayaran_Siakad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly SyncService _syncService;

        public SyncController(SyncService syncService)
        {
            _syncService = syncService;
        }

        [HttpPost]
        public async Task<IActionResult> Sync()
        {
            try
            {
                var (added, updated) = await _syncService.SyncMahasiswaAsync();

                return Ok(new
                {
                    success = true,
                    message = "Sinkronisasi data mahasiswa berhasil",
                    detail = new
                    {
                        ditambahkan = added,
                        diperbarui = updated
                    }
                });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Gagal mengambil data dari API Mahasiswa",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Terjadi kesalahan saat sinkronisasi",
                    error = ex.Message
                });
            }
        }
    }
}
