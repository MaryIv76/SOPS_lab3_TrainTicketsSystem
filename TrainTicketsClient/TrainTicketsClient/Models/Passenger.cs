namespace TrainTicketsClient.Models
{
    public class Passenger
    {
        public int seatId { get; set; }
        public int seatNumber { get; set; }
        public string type { get; set; }
        public double price { get; set; }
        public String? surname { get; set; }
        public String? firstName { get; set; }
        public String? thirdName { get; set; }
    }
}
