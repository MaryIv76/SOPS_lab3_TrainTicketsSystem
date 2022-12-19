using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrainTicketsServer.Models.Entities
{
    [Table(name: "trip")]
    public class Trip
    {
        [Key]
        public int trip_id { get; set; }
        public int route_id { get; set; }
        public int seat { get; set; }
        public double price { get; set; }
        public string date { get; set; }
    }
}
