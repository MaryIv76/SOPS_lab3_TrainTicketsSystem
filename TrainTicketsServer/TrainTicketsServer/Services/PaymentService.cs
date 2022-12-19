using Grpc.Core;
using TrainTicketsServer.Models;
using TrainTicketsServer.Protos;

namespace TrainTicketsServer.Services
{
    public class PaymentService : BuyTicket.BuyTicketBase
    {
        ApplicationContext db;
        DBConnector dbConnector;

        public PaymentService(ApplicationContext context)
        {
            db = context;
            dbConnector = new DBConnector(db);
        }

        public override Task<BuyTicketReply> BuyTicket(BuyTicketRequest request, ServerCallContext context)
        {
            var reply = new BuyTicketReply();
            reply.ServicePaid = true;
            reply.TicketPaid = dbConnector.insertTicket(request.UserId, request.TripId, request.Surname, request.Firstname, request.Thirdname);
            return Task.FromResult(reply);
        }
    }
}
