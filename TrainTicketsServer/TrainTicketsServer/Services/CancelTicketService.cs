using Grpc.Core;
using System.Data.Common;
using System.Numerics;
using System;
//using TrainTicketsServer.Models;
using TrainTicketsServer.Protos;

namespace TrainTicketsServer.Services
{
    public class CancelTicketService : CancelTicket.CancelTicketBase
    {

        public override Task<CancelTicketReply> CancelTicket(CancelTicketRequest request, ServerCallContext context)
        {

        }
    }
}
