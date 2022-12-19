using TrainTicketsClient.Models;
using TrainTicketsServer.Protos;
using Google.Protobuf.WellKnownTypes;

namespace TrainTicketsClient.Controllers
{
    public class HomeControllerHelper
    {

        public static string token = "myToken";
        public static bool CheckLoginInput(LoginData loginData, out String invalidParameterInfo)
        {
            if (loginData.login == null || loginData.login == "")
            {
                invalidParameterInfo = "Invalid login";
                return false;
            }
            if (loginData.password == null || loginData.password == "")
            {
                invalidParameterInfo = "Invalid password";
                return false;
            }
            invalidParameterInfo = "";
            return true;
        }

        public static LoginRequest FromLoginDataToLoginRequest(LoginData loginData)
        {
            LoginRequest loginRequest = new LoginRequest();
            loginRequest.Token = token;
            loginRequest.Login = loginData.login;
            loginRequest.Password = loginData.password;
            return loginRequest;
        }

        public static LoginReplyClass FromLoginReplyToLoginReplyClass(LoginReply reply)
        {
            LoginReplyClass loginReplyClass = new LoginReplyClass();
            loginReplyClass.servicePaid = reply.ServicePaid;
            loginReplyClass.loginSuccessful = reply.LoginSuccessful;
            loginReplyClass.userId = reply.UserId;
            return loginReplyClass;
        }

        public static bool CheckLoginReply(LoginReplyClass loginReplyClass, out String invalidParameterInfo)
        {
            if (!loginReplyClass.servicePaid)
            {
                invalidParameterInfo = "Service wasn't paid for";
                return false;
            }
            if (!loginReplyClass.loginSuccessful)
            {
                invalidParameterInfo = "Invalid login or password";
                return false;
            }
            invalidParameterInfo = "";
            return true;
        }

        public static bool CheckFindTrainInput(FindTrain findTrain, out String invalidParameterInfo)
        {
            if (findTrain.date == null)
            {
                invalidParameterInfo = "Invalid date";
                return false;
            }
            if (findTrain.departure == null || findTrain.departure == "")
            {
                invalidParameterInfo = "Invalid departure";
                return false;
            }
            if (findTrain.destination == null || findTrain.destination == "")
            {
                invalidParameterInfo = "Invalid destination";
                return false;
            }
            invalidParameterInfo = "";
            return true;
        }

        public static TrainsRequest FromFindTrainToTrainsRequest(FindTrain findTrain)
        {
            TrainsRequest trainsRequest = new TrainsRequest();
            trainsRequest.Token = token;
            trainsRequest.From = findTrain.departure;
            trainsRequest.To = findTrain.destination;
            trainsRequest.Date = Timestamp.FromDateTime(DateTime.SpecifyKind((DateTime)findTrain.date, DateTimeKind.Utc));
            return trainsRequest;
        }

        public static List<RouteClass> FromTrainsReplyToRouteClassList(TrainsReply reply, DateTime date)
        {
            List<RouteClass> routes = new List<RouteClass>();
            foreach (RouteTrain route in reply.Routes)
            {
                RouteClass curRoute = new RouteClass();
                curRoute.routeId = route.RouteId;
                curRoute.arrivalTime = route.ArrivalTime.ToDateTime().ToString("HH:mm");
                curRoute.departureTime = route.DepartureTime.ToDateTime().ToString("HH:mm");
                curRoute.date = date;
                routes.Add(curRoute);
            }
            return routes;
        }

        public static bool CheckTrainsReply(TrainsReply reply, out String invalidParameterInfo)
        {
            if (!reply.ServicePaid)
            {
                invalidParameterInfo = "Service wasn't paid for";
                return false;
            }
            invalidParameterInfo = "";
            return true;
        }

        public static SeatsRequest FromIdToSeatsRequest(int id, DateTime date)
        {
            SeatsRequest seatsRequest = new SeatsRequest();
            seatsRequest.Token = token;
            seatsRequest.RouteId = id;
            seatsRequest.Date = Timestamp.FromDateTime(DateTime.SpecifyKind((DateTime)date, DateTimeKind.Utc));
            return seatsRequest;
        }

        public static bool CheckSeatsReply(SeatsReply reply, out String invalidParameterInfo)
        {
            if (!reply.ServicePaid)
            {
                invalidParameterInfo = "Service wasn't paid for";
                return false;
            }
            invalidParameterInfo = "";
            return true;
        }

        public static List<TrainTicketsClient.Models.Seat> FromSeatsReplyToSeatList(SeatsReply reply)
        {
            List<TrainTicketsClient.Models.Seat> seats = new List<TrainTicketsClient.Models.Seat>();
            foreach (TrainTicketsServer.Protos.Seat seat in reply.Seats)
            {
                TrainTicketsClient.Models.Seat curSeat = new TrainTicketsClient.Models.Seat();
                curSeat.id = seat.Id;
                curSeat.seatNumber = seat.SeatNumber;
                curSeat.type = seat.Type;
                curSeat.price = seat.Price;
                seats.Add(curSeat);
            }
            return seats;
        }

        public static bool CheckPassengerInput(Passenger passenger, out String invalidParameterInfo)
        {
            if (passenger.surname == null || passenger.surname == "")
            {
                invalidParameterInfo = "Invalid surname";
                return false;
            }
            if (passenger.firstName == null || passenger.firstName == "")
            {
                invalidParameterInfo = "Invalid first name";
                return false;
            }
            if (passenger.thirdName == null || passenger.thirdName == "")
            {
                invalidParameterInfo = "Invalid third name";
                return false;
            }
            invalidParameterInfo = "";
            return true;
        }

        public static BuyTicketRequest FromPassengerToBuyTicketRequest(Passenger passenger)
        {
            BuyTicketRequest buyTicketRequest = new BuyTicketRequest();
            buyTicketRequest.Token = token;
            buyTicketRequest.SeatId = passenger.seatId;
            buyTicketRequest.Surname = passenger.surname;
            buyTicketRequest.Firstname = passenger.firstName;
            buyTicketRequest.Thirdname = passenger.thirdName;
            return buyTicketRequest;
        }

        public static bool CheckBuyTicketReply(BuyTicketReply reply, out String invalidParameterInfo)
        {
            if (!reply.ServicePaid)
            {
                invalidParameterInfo = "Service wasn't paid for";
                return false;
            }
            if (!reply.TicketPaid)
            {
                invalidParameterInfo = "Ticket wasn't paid for";
                return false;
            }
            invalidParameterInfo = "";
            return true;
        }

        public static GetMyTicketsRequest FromUserIdToGetMyTicketsRequest(int userId)
        {
            GetMyTicketsRequest getMyTicketsRequest = new GetMyTicketsRequest();
            getMyTicketsRequest.Token = token;
            getMyTicketsRequest.UserId = userId;
            return getMyTicketsRequest;
        }

        public static bool CheckGetMyTicketsReply(GetMyTicketsReply reply, out String invalidParameterInfo)
        {
            if (!reply.ServicePaid)
            {
                invalidParameterInfo = "Service wasn't paid for";
                return false;
            }
            invalidParameterInfo = "";
            return true;
        }

        public static List<TrainTicketsClient.Models.Ticket> FromGetMyTicketsReplyToTicketList(GetMyTicketsReply reply)
        {
            List<TrainTicketsClient.Models.Ticket> tickets = new List<TrainTicketsClient.Models.Ticket>();
            foreach (TrainTicketsServer.Protos.Ticket ticket in reply.MyTickets)
            {
                TrainTicketsClient.Models.Ticket curTicket = new TrainTicketsClient.Models.Ticket();
                curTicket.ticketNumber = ticket.TicketNumber;
                curTicket.surname = ticket.Surname;
                curTicket.firstname = ticket.Firstname;
                curTicket.thirdname = ticket.Thirdname;
                curTicket.from = ticket.From;
                curTicket.to = ticket.To;
                curTicket.departureTime = ticket.DepartureTime.ToDateTime().ToString("HH:mm");
                curTicket.arrivalTime = ticket.ArrivalTime.ToDateTime().ToString("HH:mm");
                curTicket.date = ticket.Date.ToDateTime().ToString("dd.MM.yyyy");
                curTicket.seatNumber = ticket.SeatNumber;
                curTicket.price = ticket.Price;
                tickets.Add(curTicket);
            }
            return tickets;
        }

        public static CancelTicketRequest FromTicketNumberToCancelTicketRequest(int ticketNumber)
        {
            CancelTicketRequest cancelTicketRequest = new CancelTicketRequest();
            cancelTicketRequest.Token = token;
            cancelTicketRequest.TicketNumber = ticketNumber;
            return cancelTicketRequest;
        }

        public static bool CheckCancelTicketReply(CancelTicketReply reply, out String invalidParameterInfo)
        {
            if (!reply.ServicePaid)
            {
                invalidParameterInfo = "Service wasn't paid for";
                return false;
            }
            if (!reply.TicketCanceled)
            {
                invalidParameterInfo = "Ticket wasn't deleted";
                return false;
            }
            invalidParameterInfo = "";
            return true;
        }

        public static bool CheckChangePassengerInfo(ChangePassengerInfo changePassengerInfo, out String invalidParameterInfo)
        {
            if (changePassengerInfo.passengerSurname == null || changePassengerInfo.passengerSurname == "")
            {
                invalidParameterInfo = "Invalid surname";
                return false;
            }
            if (changePassengerInfo.passengerFirstName == null || changePassengerInfo.passengerFirstName == "")
            {
                invalidParameterInfo = "Invalid first name";
                return false;
            }
            if (changePassengerInfo.passengerThirdName == null || changePassengerInfo.passengerThirdName == "")
            {
                invalidParameterInfo = "Invalid third name";
                return false;
            }
            invalidParameterInfo = "";
            return true;
        }

        public static ChangePassengerRequest FromChangePassengerInfoToChangePassengerRequest(ChangePassengerInfo changePassengerInfo)
        {
            ChangePassengerRequest changePassengerRequest = new ChangePassengerRequest();
            changePassengerRequest.Token = token;
            changePassengerRequest.TicketNumber = (int)changePassengerInfo.ticketNumber;
            changePassengerRequest.Surname = changePassengerInfo.passengerSurname;
            changePassengerRequest.Firstname = changePassengerInfo.passengerFirstName;
            changePassengerRequest.Thirdname = changePassengerInfo.passengerThirdName;
            return changePassengerRequest;
        }

        public static bool CheckChangePassengerReply(ChangePassengerReply reply, out String invalidParameterInfo)
        {
            if (!reply.ServicePaid)
            {
                invalidParameterInfo = "Service wasn't paid for";
                return false;
            }
            if (!reply.PassengerChanged)
            {
                invalidParameterInfo = "Passenger wasn't changed";
                return false;
            }
            invalidParameterInfo = "";
            return true;
        }
    }
}
