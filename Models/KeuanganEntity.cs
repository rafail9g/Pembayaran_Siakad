using System.ComponentModel.DataAnnotations;

namespace Pembayaran_Siakad.Models
{
    public class KeuanganEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string MahasiswaId { get; set; } = string.Empty;

        public decimal NilaiUkt { get; set; }

        public string StatusTagihan { get; set; } = "Belum Lunas";

        public DateTime TanggalTagihan { get; set; }

        public DateTime? TanggalJatuhTempo { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public MahasiswaEntity? Mahasiswa { get; set; }

        public List<RiwayatPembayaranEntity> RiwayatPembayaran { get; set; } = new();
    }
}
