using Grpc.Core;
using System.Data.Common;
using System.Numerics;
using System;
//using TrainTicketsServer.Models;
using TrainTicketsServer.Protos;

namespace TrainTicketsServer.Services
{
    public class SeatsInfoService : Seats.SeatsBase
    {

        public override Task<SeatsReply> GetAvailableSeats(SeatsRequest request, ServerCallContext context)
        {

        }
    }
}