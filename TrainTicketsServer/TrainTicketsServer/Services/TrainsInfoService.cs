using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TrainTicketsServer.Models;
using TrainTicketsServer.Protos;

namespace TrainTicketsServer.Services
{
    public class TrainsInfoService : TrainsInfo.TrainsInfoBase
    {
        ApplicationContext db;
        DBConnector dbConnector;

        public TrainsInfoService(ApplicationContext context)
        {
            db = context;
            dbConnector = new DBConnector(db);
        }

        public override Task<TrainsReply> GetAvailableTrains(TrainsRequest request, ServerCallContext context)
        {
            DateOnly date = DateOnly.FromDateTime(request.Date.ToDateTime());
            List<TrainTicketsServer.Models.Entities.Route> availableTrains = dbConnector.getAvailableTrains(request.From, request.To, date);
            var reply = FormReply(availableTrains);
            return Task.FromResult(reply);
        }

        private TrainsReply FormReply(List<TrainTicketsServer.Models.Entities.Route> availableTrains)
        {
            TrainsReply reply = new TrainsReply();
            foreach( var train in availableTrains)
            {
                RouteTrain routeTrain = new RouteTrain();
                routeTrain.RouteId = train.route_id;
                routeTrain.DepartureTime = Timestamp.FromDateTime(DateTime.SpecifyKind(train.departure_time, DateTimeKind.Utc));
                routeTrain.ArrivalTime = Timestamp.FromDateTime(DateTime.SpecifyKind(train.arrival_time, DateTimeKind.Utc));
                reply.Routes.Add(routeTrain);
            }
            reply.ServicePaid = true;
            return reply;
        }
    }
}
