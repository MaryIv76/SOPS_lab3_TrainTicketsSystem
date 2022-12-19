namespace TrainTicketsClient.Models
{
    public class Seat
    {
        public int tripId { get; set; }
        public int seatNumber { get; set; }
        public double price { get; set; }
        public string type { get; set; }
    }
}
