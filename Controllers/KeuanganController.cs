using Microsoft.AspNetCore.Mvc;
using Pembayaran_Siakad.Models;
using Pembayaran_Siakad.Services;

namespace Pembayaran_Siakad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KeuanganController : ControllerBase
    {
        private readonly KeuanganService _keuanganService;

        public KeuanganController(KeuanganService keuanganService)
        {
            _keuanganService = keuanganService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _keuanganService.GetAllAsync();

            return Ok(new
            {
                success = true,
                count = data.Count,
                data
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var data = await _keuanganService.GetByIdAsync(id);

            if (data == null)
                return NotFound(new { success = false, message = "Data keuangan tidak ditemukan" });

            return Ok(new { success = true, data });
        }

        [HttpGet("mahasiswa/{mahasiswaId}")]
        public async Task<IActionResult> GetByMahasiswaId(string mahasiswaId)
        {
            var data = await _keuanganService.GetByMahasiswaIdAsync(mahasiswaId);

            if (data == null)
                return NotFound(new
                {
                    success = false,
                    message = "Data keuangan untuk mahasiswa ini tidak ditemukan"
                });

            return Ok(new { success = true, data });
        }


        [HttpPost]
        public async Task<IActionResult> TambahKeuangan([FromBody] TambahKeuanganRequest request)
        {
            if (request.NilaiUkt <= 0)
                return BadRequest(new { success = false, message = "Nilai UKT harus lebih dari 0" });

            var result = await _keuanganService.TambahKeuanganAsync(request);

            if (result == null)
                return BadRequest(new
                {
                    success = false,
                    message = "Mahasiswa tidak ditemukan atau sudah memiliki data keuangan. Pastikan sudah melakukan sinkronisasi."
                });

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, new
            {
                success = true,
                message = "Data keuangan berhasil ditambahkan",
                data = result
            });
        }

        [HttpPost("{keuanganId}/pembayaran")]
        public async Task<IActionResult> TambahPembayaran(
            string keuanganId,
            [FromBody] TambahPembayaranRequest request)
        {
            if (request.JumlahBayar <= 0)
                return BadRequest(new { success = false, message = "Jumlah bayar harus lebih dari 0" });

            if (string.IsNullOrWhiteSpace(request.MetodePembayaran))
                return BadRequest(new { success = false, message = "Metode pembayaran tidak boleh kosong" });

            var result = await _keuanganService.TambahPembayaranAsync(keuanganId, request);

            if (result == null)
                return NotFound(new { success = false, message = "Data keuangan tidak ditemukan" });

            return Ok(new
            {
                success = true,
                message = "Pembayaran berhasil dicatat",
                data = result
            });
        }
    }
}
