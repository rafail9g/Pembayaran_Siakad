namespace Pembayaran_Siakad.Models
{

    public class MahasiswaApiResponse
    {
        public bool Success { get; set; }
        public int Count { get; set; }
        public List<MahasiswaApiData> Data { get; set; } = new();
    }

    public class MahasiswaApiData
    {
        public string _id { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public string ProgramStudi { get; set; } = string.Empty;
        public string StatusAkademik { get; set; } = string.Empty;
        public List<string> MataKuliah { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }


    public class KeuanganResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string MahasiswaId { get; set; } = string.Empty;
        public string NamaMahasiswa { get; set; } = string.Empty;
        public string ProgramStudi { get; set; } = string.Empty;
        public string StatusAkademik { get; set; } = string.Empty;
        public decimal NilaiUkt { get; set; }
        public string StatusTagihan { get; set; } = string.Empty;
        public DateTime TanggalTagihan { get; set; }
        public DateTime? TanggalJatuhTempo { get; set; }
        public List<RiwayatPembayaranDto> RiwayatPembayaran { get; set; } = new();
    }

    public class RiwayatPembayaranDto
    {
        public string Id { get; set; } = string.Empty;
        public decimal JumlahBayar { get; set; }
        public DateTime TanggalBayar { get; set; }
        public string MetodePembayaran { get; set; } = string.Empty;
        public string Keterangan { get; set; } = string.Empty;
    }


    public class TambahPembayaranRequest
    {
        public decimal JumlahBayar { get; set; }
        public string MetodePembayaran { get; set; } = string.Empty;
        public string Keterangan { get; set; } = string.Empty;
    }

    public class TambahKeuanganRequest
    {
        public string MahasiswaId { get; set; } = string.Empty;
        public decimal NilaiUkt { get; set; }
        public DateTime? TanggalJatuhTempo { get; set; }
    }
}
