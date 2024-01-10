using INZ_API_POSTGRES.Schemas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INZ_API_POSTGRES.Schemas
{
    public class Devices
    {
        [Key] 
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mac { get; set; }
        public ICollection<Data> Datas { get; } = new List<Data>();

        public string UserId { get; set; }

        [ForeignKey("UserId")]


        public string ApiKey { get; set; }
        [ForeignKey("ApiKey")]

        public Users Users { get; set; }

    }
}
