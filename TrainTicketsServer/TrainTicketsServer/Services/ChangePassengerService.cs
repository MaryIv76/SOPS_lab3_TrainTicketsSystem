using Grpc.Core;
using System.Data.Common;
using System.Numerics;
using System;
//using TrainTicketsServer.Models;
using TrainTicketsServer.Protos;

namespace TrainTicketsServer.Services
{
    public class ChangePassengerService : ChangePassenger.ChangePassengerBase
    {

        public override Task<ChangePassengerReply> ChangePassenger(ChangePassengerRequest request, ServerCallContext context)
        {

        }
    }
}
