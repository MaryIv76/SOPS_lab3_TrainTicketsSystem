namespace TrainTicketsServer.Models.Entities
{
    public class SeatInfo
    {
        public int trip_id { get; set; }
        public int seat_number { get; set; }
        public double price { get; set; }
        public string seat_type { get; set; }
    }
}
