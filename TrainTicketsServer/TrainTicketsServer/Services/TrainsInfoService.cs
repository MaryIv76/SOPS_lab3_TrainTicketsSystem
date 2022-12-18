using Grpc.Core;
using System.Data.Common;
using System.Numerics;
using System;
//using TrainTicketsServer.Models;
using TrainTicketsServer.Protos;

namespace TrainTicketsServer.Services
{
    public class TrainsInfoService : Trains.TrainsBase
    {

        public override Task<TrainsReply> GetAvailableTrains(TrainsRequest request, ServerCallContext context)
        {

        }
    }
}
