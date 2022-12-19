using Grpc.Core;
using System.Data.Common;
using System.Numerics;
using System;
using TrainTicketsServer.Models;
using TrainTicketsServer.Protos;

namespace TrainTicketsServer.Services
{
    public class LoginService : Login.LoginBase
    {
        ApplicationContext db;
        DBConnector dbConnector;

        public LoginService(ApplicationContext context)
        {
            db = context;
            dbConnector = new DBConnector(db);
        }

        public override Task<LoginReply> LoginUser(LoginRequest request, ServerCallContext context)
        {
            int userId = -1;
            bool isValidUserData = dbConnector.checkLoginData(request.Login, request.Password, ref userId);
            var reply = new LoginReply();
            reply.ServicePaid = true;
            reply.LoginSuccessful = isValidUserData;
            reply.UserId = userId;
            return Task.FromResult(reply);
        }
    }
}

