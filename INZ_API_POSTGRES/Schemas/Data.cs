using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INZ_API_POSTGRES.Schemas
{
    public class Data
    {
        [Key] public int Id { get; set; }
        
        public string Mac { get; set; }

        public DateTime Date { get; set; }
        
        public int DeviceId { get; set; }

        [ForeignKey("DeviceId")]
        public required Devices Device { get; set; }
    }
}
