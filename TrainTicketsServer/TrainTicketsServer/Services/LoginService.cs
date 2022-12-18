using Grpc.Core;
using System.Data.Common;
using System.Numerics;
using System;
//using TrainTicketsServer.Models;
using TrainTicketsServer.Protos;

namespace TrainTicketsServer.Services
{
    public class LoginService : Login.LoginBase
    {
        /*ApplicationContext db;
        DBConnector dbConnector;
        public SelectPlayerService(ApplicationContext context)
        {
            db = context;
            dbConnector = new DBConnector(db);
        }*/

        public override Task<LoginReply> LoginUser(LoginRequest request, ServerCallContext context)
        {
            /*FindPlayer findPlayer = FromSelectPlayersRequestToFindPlayer(request);
            List<Player> playerByRequest = dbConnector.getPlayerByQuery(findPlayer);

            var players = FromPlayerListToSelectPlayersReply(playerByRequest);
            return Task.FromResult(players);*/
        }
    }
}

