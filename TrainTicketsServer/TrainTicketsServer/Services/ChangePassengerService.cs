using Grpc.Core;
using TrainTicketsServer.Protos;
using TrainTicketsServer.Models;

namespace TrainTicketsServer.Services
{
    public class ChangePassengerService : ChangePassenger.ChangePassengerBase
    {
        ApplicationContext db;
        DBConnector dbConnector;

        public ChangePassengerService(ApplicationContext context)
        {
            db = context;
            dbConnector = new DBConnector(db);
        }

        public override Task<ChangePassengerReply> ChangePassenger(ChangePassengerRequest request, ServerCallContext context)
        {
            var reply = new ChangePassengerReply();
            reply.ServicePaid = true;
            reply.PassengerChanged = dbConnector.updateTicket(request.TicketNumber, request.Surname, request.Firstname, request.Thirdname);
            return Task.FromResult(reply);
        }
    }
}
