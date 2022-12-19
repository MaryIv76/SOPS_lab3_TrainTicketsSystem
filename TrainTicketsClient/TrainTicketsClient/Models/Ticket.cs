namespace TrainTicketsClient.Models
{
    public class Ticket
    {
        public int ticketNumber { get; set; }
        public String? surname { get; set; }
        public String? firstname { get; set; }
        public String? thirdname { get; set; }
        public String? from { get; set; }
        public String? to { get; set; }
        public String? departureTime { get; set; }
        public String? arrivalTime { get; set; }
        public String? date { get; set; }
        public int seatNumber { get; set; }
        public double price { get; set; }
    }
}
