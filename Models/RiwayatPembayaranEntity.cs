using System.ComponentModel.DataAnnotations;

namespace Pembayaran_Siakad.Models
{
    public class RiwayatPembayaranEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string KeuanganId { get; set; } = string.Empty;

        public decimal JumlahBayar { get; set; }

        public DateTime TanggalBayar { get; set; } = DateTime.UtcNow;

        public string MetodePembayaran { get; set; } = string.Empty;

        public string Keterangan { get; set; } = string.Empty;

        public KeuanganEntity? Keuangan { get; set; }
    }
}
