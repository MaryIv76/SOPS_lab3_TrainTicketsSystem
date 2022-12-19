using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTicketsServer.Models.Entities
{
    [Table(name: "ticket")]
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int number { get; set; }
        public string surname { get; set; }
        public string firstname { get; set; }
        public string thirdname { get; set; }
        public int user_id { get; set; }
        public int trip_id { get; set; }
    }
}
