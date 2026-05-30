using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pembayaran_Siakad.Data;

namespace Pembayaran_Siakad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MahasiswaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MahasiswaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Mahasiswas.ToListAsync();

            return Ok(new
            {
                success = true,
                count = data.Count,
                data = data.Select(m => new
                {
                    m.Id,
                    m.Nama,
                    m.ProgramStudi,
                    m.StatusAkademik,
                    m.MataKuliah,
                    m.CreatedAt
                })
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var mahasiswa = await _context.Mahasiswas.FindAsync(id);

            if (mahasiswa == null)
                return NotFound(new { success = false, message = "Mahasiswa tidak ditemukan" });

            return Ok(new
            {
                success = true,
                data = new
                {
                    mahasiswa.Id,
                    mahasiswa.Nama,
                    mahasiswa.ProgramStudi,
                    mahasiswa.StatusAkademik,
                    mahasiswa.MataKuliah,
                    mahasiswa.CreatedAt
                }
            });
        }
    }
}
