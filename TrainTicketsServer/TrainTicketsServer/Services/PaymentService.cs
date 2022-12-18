using Grpc.Core;
using System.Data.Common;
using System.Numerics;
using System;
//using TrainTicketsServer.Models;
using TrainTicketsServer.Protos;

namespace TrainTicketsServer.Services
{
    public class PaymentService : BuyTicket.BuyTicketBase
    {

        public override Task<BuyTicketReply> BuyTicket(BuyTicketRequest request, ServerCallContext context)
        {

        }
    }
}
