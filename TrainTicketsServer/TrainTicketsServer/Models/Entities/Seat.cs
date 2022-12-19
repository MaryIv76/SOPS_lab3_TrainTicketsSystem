using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTicketsServer.Models.Entities
{
    [Table(name: "seat")]
    public class Seat
    {
        [Key]
        public int seat_id { get; set; }
        public int seat_number { get; set; }
        public string type { get; set; }
    }
}
