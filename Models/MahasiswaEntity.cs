using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pembayaran_Siakad.Models
{
    public class MahasiswaEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        public string Nama { get; set; } = string.Empty;

        public string ProgramStudi { get; set; } = string.Empty;

        public string StatusAkademik { get; set; } = string.Empty;

        public string MataKuliahJson { get; set; } = "[]";

        [NotMapped]
        public List<string> MataKuliah
        {
            get => System.Text.Json.JsonSerializer.Deserialize<List<string>>(MataKuliahJson) ?? new();
            set => MataKuliahJson = System.Text.Json.JsonSerializer.Serialize(value);
        }

        public DateTime CreatedAt { get; set; }
    }
}
