using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrainTicketsServer.Models.Entities
{
    [Table(name: "route")]
    public class Route
    {
        [Key]
        public int route_id { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public DateTime departure_time { get; set; }
        public DateTime arrival_time { get; set; }

    }
}
