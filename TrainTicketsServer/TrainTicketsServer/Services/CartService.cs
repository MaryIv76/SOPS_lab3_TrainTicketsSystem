using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TrainTicketsServer.Models;
using TrainTicketsServer.Models.Entities;
using TrainTicketsServer.Protos;

namespace TrainTicketsServer.Services
{
    public class CartService : MyTicketsInfo.MyTicketsInfoBase
    {
        ApplicationContext db;
        DBConnector dbConnector;

        public CartService(ApplicationContext context)
        {
            db = context;
            dbConnector = new DBConnector(db);
        }

        public override Task<GetMyTicketsReply> GetMyTickets(GetMyTicketsRequest request, ServerCallContext context)
        {
            List<TicketInfo> myTickets = dbConnector.getTicketsByUser(request.UserId);
            var reply = FormReply(myTickets);
            return Task.FromResult(reply);
        }

        private GetMyTicketsReply FormReply(List<TicketInfo> myTickets)
        {
            GetMyTicketsReply reply = new GetMyTicketsReply();
            foreach (var ticketInfo in myTickets)
            {
                Protos.Ticket ticketReply = new Protos.Ticket();
                ticketReply.TicketNumber = ticketInfo.ticketNumber;
                ticketReply.Surname = ticketInfo.surname;
                ticketReply.Firstname = ticketInfo.firstname;
                ticketReply.Thirdname = ticketInfo.thirdname;
                ticketReply.From = ticketInfo.from;
                ticketReply.To = ticketInfo.to;
                ticketReply.DepartureTime = Timestamp.FromDateTime(DateTime.SpecifyKind(ticketInfo.departureTime, DateTimeKind.Utc));
                ticketReply.ArrivalTime = Timestamp.FromDateTime(DateTime.SpecifyKind(ticketInfo.arrivalTime, DateTimeKind.Utc));
                ticketReply.Date = Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.Parse(ticketInfo.date), DateTimeKind.Utc));
                ticketReply.SeatNumber = ticketInfo.seatNumber;
                ticketReply.Price = ticketInfo.price;
                reply.MyTickets.Add(ticketReply);
            }
            reply.ServicePaid = true;
            return reply;
        }
    }
}
