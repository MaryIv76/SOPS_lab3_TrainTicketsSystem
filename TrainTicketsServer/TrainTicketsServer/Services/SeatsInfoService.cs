using Grpc.Core;
using TrainTicketsServer.Models;
using TrainTicketsServer.Protos;
using TrainTicketsServer.Models.Entities;
using Google.Protobuf.WellKnownTypes;

namespace TrainTicketsServer.Services
{
    public class SeatsInfoService : SeatsInfo.SeatsInfoBase
    {
        ApplicationContext db;
        DBConnector dbConnector;

        public SeatsInfoService(ApplicationContext context)
        {
            db = context;
            dbConnector = new DBConnector(db);
        }

        public override Task<SeatsReply> GetAvailableSeats(SeatsRequest request, ServerCallContext context)
        {
            DateOnly date = DateOnly.FromDateTime(request.Date.ToDateTime());
            List<SeatInfo> availableSeats = dbConnector.getAvailableSeats(request.RouteId, date);
            var reply = FormReply(availableSeats);
            return Task.FromResult(reply);
        }

        private SeatsReply FormReply(List<SeatInfo> availableSeats)
        {
            SeatsReply reply = new SeatsReply();
            foreach (var seatInfo in availableSeats)
            {
                TrainTicketsServer.Protos.Seat seat = new TrainTicketsServer.Protos.Seat();
                seat.SeatNumber = seatInfo.seat_number;
                seat.Price = seatInfo.price;
                seat.Type = seatInfo.seat_type;
                seat.TripId = seatInfo.trip_id;
                reply.Seats.Add(seat);
            }
            reply.ServicePaid = true;
            return reply;
        }
    }
}