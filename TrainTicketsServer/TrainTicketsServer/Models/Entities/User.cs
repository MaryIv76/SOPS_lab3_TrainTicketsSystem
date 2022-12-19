using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrainTicketsServer.Models.Entities
{
    [Table(name: "user")]
    public class User
    {
        [Key]
        public int user_id{ get; set; }
        public string login { get; set; }
        public string password { get; set; }
    }
}
