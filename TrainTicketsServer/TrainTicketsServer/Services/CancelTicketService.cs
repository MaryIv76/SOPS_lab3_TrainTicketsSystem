using Grpc.Core;
using TrainTicketsServer.Models;
using TrainTicketsServer.Protos;

namespace TrainTicketsServer.Services
{
    public class CancelTicketService : CancelTicket.CancelTicketBase
    {
        ApplicationContext db;
        DBConnector dbConnector;

        public CancelTicketService(ApplicationContext context)
        {
            db = context;
            dbConnector = new DBConnector(db);
        }

        public override Task<CancelTicketReply> CancelTicket(CancelTicketRequest request, ServerCallContext context)
        {
            var reply = new CancelTicketReply();
            reply.ServicePaid = true;
            reply.TicketCanceled = dbConnector.deleteTicket(request.TicketNumber);
            return Task.FromResult(reply);

        }
    }
}
