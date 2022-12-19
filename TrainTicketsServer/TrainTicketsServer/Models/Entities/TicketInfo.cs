namespace TrainTicketsServer.Models.Entities
{
    public class TicketInfo
    {
        public int ticketNumber { get; set; }
        public string surname { get; set; }
        public string firstname { get; set; }
        public string thirdname { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public DateTime departureTime { get; set; }
        public DateTime arrivalTime { get; set; }
        public string date { get; set; }
        public int seatNumber { get; set; }
        public double price { get; set; }
    }
}
