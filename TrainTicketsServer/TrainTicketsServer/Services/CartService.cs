using Grpc.Core;
using System.Data.Common;
using System.Numerics;
using System;
//using TrainTicketsServer.Models;
using TrainTicketsServer.Protos;

namespace TrainTicketsServer.Services
{
    public class CartService : MyTicketsInfo.MyTicketsInfoBase
    {

        public override Task<GetMyTicketsReply> GetMyTickets(GetMyTicketsRequest request, ServerCallContext context)
        {

        }
    }
}
