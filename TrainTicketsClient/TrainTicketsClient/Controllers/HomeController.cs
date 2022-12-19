using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TrainTicketsClient.Models;
using TrainTicketsServer.Protos;

namespace TrainTicketsClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        static int userId;
        static List<TrainTicketsClient.Models.Ticket> myTickets;
        static String invalidDeleteInfo;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            invalidDeleteInfo = "";
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginData loginData)
        {
            String invalidParameterInfo;
            if (!HomeControllerHelper.CheckLoginInput(loginData, out invalidParameterInfo))
            {
                ViewBag.Message = invalidParameterInfo;
                return View();
            }

            using var channel = GrpcChannel.ForAddress("https://localhost:7146");
            var client = new Login.LoginClient(channel);

            LoginRequest loginRequest = HomeControllerHelper.FromLoginDataToLoginRequest(loginData);
            var reply = client.LoginUser(loginRequest);

            LoginReplyClass loginReplyClass = HomeControllerHelper.FromLoginReplyToLoginReplyClass(reply);
            if (!HomeControllerHelper.CheckLoginReply(loginReplyClass, out invalidParameterInfo))
            {
                ViewBag.Message = invalidParameterInfo;
                return View();
            }
            userId = loginReplyClass.userId;
            return Redirect("/Home/Trains");
        }

        public IActionResult Trains()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Trains(FindTrain findTrain)
        {
            String invalidParameterInfo = "";
            if (!HomeControllerHelper.CheckFindTrainInput(findTrain, out invalidParameterInfo))
            {
                ViewBag.Message = invalidParameterInfo;
                return View();
            }

            using var channel = GrpcChannel.ForAddress("https://localhost:7146");
            var client = new TrainsInfo.TrainsInfoClient(channel);

            TrainsRequest trainsRequest = HomeControllerHelper.FromFindTrainToTrainsRequest(findTrain);
            var reply = client.GetAvailableTrains(trainsRequest);

            if (!HomeControllerHelper.CheckTrainsReply(reply, out invalidParameterInfo))
            {
                ViewBag.Message = invalidParameterInfo;
                return View();
            }
            List<RouteClass> routes = HomeControllerHelper.FromTrainsReplyToRouteClassList(reply, (DateTime)findTrain.date);

            ViewBag.Message = invalidParameterInfo;
            return View(routes);
        }

        public IActionResult ChooseSeat(int id, DateTime date)
        {
            String invalidParameterInfo = "";
            using var channel = GrpcChannel.ForAddress("https://localhost:7146");
            var client = new SeatsInfo.SeatsInfoClient(channel);

            SeatsRequest seatsRequest = HomeControllerHelper.FromIdToSeatsRequest(id, date);
            var reply = client.GetAvailableSeats(seatsRequest);

            if (!HomeControllerHelper.CheckSeatsReply(reply, out invalidParameterInfo))
            {
                ViewBag.Message = invalidParameterInfo;
                return View();
            }
            List<TrainTicketsClient.Models.Seat> seats = HomeControllerHelper.FromSeatsReplyToSeatList(reply);

            ViewBag.Message = invalidParameterInfo;
            return View(seats);
        }

        public IActionResult BuyTicket(int tripId, int seatNumber, string type, double price)
        {
            ViewBag.TripId = tripId;
            ViewBag.SeatNumber = seatNumber;
            ViewBag.Type = type;
            ViewBag.Price = price;
            return View();
        }

        [HttpPost]
        public IActionResult BuyTicket(Passenger passenger)
        {
            String invalidParameterInfo;
            if (!HomeControllerHelper.CheckPassengerInput(passenger, out invalidParameterInfo))
            {
                ViewBag.TripId = passenger.tripId;
                ViewBag.SeatNumber = passenger.seatNumber;
                ViewBag.Type = passenger.type;
                ViewBag.Price = passenger.price;
                ViewBag.Message = invalidParameterInfo;
                return View();
            }

            using var channel = GrpcChannel.ForAddress("https://localhost:7146");
            var client = new BuyTicket.BuyTicketClient(channel);

            BuyTicketRequest buyTicketRequest = HomeControllerHelper.FromPassengerToBuyTicketRequest(passenger, userId);
            var reply = client.BuyTicket(buyTicketRequest);

            if (!HomeControllerHelper.CheckBuyTicketReply(reply, out invalidParameterInfo))
            {
                ViewBag.TripId = passenger.tripId;
                ViewBag.SeatNumber = passenger.seatNumber;
                ViewBag.Type = passenger.type;
                ViewBag.Price = passenger.price;
                ViewBag.Message = invalidParameterInfo;
                return View();
            }

            return Redirect("/Home/Cart");
        }

        public IActionResult Cart()
        {
            String invalidParameterInfo = "";

            using var channel = GrpcChannel.ForAddress("https://localhost:7146");
            var client = new MyTicketsInfo.MyTicketsInfoClient(channel);

            GetMyTicketsRequest getMyTicketsRequest = HomeControllerHelper.FromUserIdToGetMyTicketsRequest(userId);
            var reply = client.GetMyTickets(getMyTicketsRequest);

            if (!HomeControllerHelper.CheckGetMyTicketsReply(reply, out invalidParameterInfo))
            {
                ViewBag.Message = invalidParameterInfo;
                return View();
            }

            myTickets = HomeControllerHelper.FromGetMyTicketsReplyToTicketList(reply);

            if (invalidDeleteInfo != "")
            {
                ViewBag.Message = invalidDeleteInfo;
                invalidDeleteInfo = "";
                return View(myTickets);
            }

            ViewBag.Message = invalidParameterInfo;
            return View(myTickets);
        }

        public IActionResult CancelTicket(int ticketNumber)
        {
            invalidDeleteInfo = "";
            String invalidParameterInfo = "";

            using var channel = GrpcChannel.ForAddress("https://localhost:7146");
            var client = new CancelTicket.CancelTicketClient(channel);

            CancelTicketRequest cancelTicketRequest = HomeControllerHelper.FromTicketNumberToCancelTicketRequest(ticketNumber);
            var reply = client.CancelTicket(cancelTicketRequest);

            if (!HomeControllerHelper.CheckCancelTicketReply(reply, out invalidParameterInfo))
            {
                invalidDeleteInfo = invalidParameterInfo;
                return Redirect("/Home/Cart");
            }

            invalidDeleteInfo = invalidParameterInfo;
            return Redirect("/Home/Cart");
        }

        [HttpPost]
        public IActionResult Cart(ChangePassengerInfo changePassengerInfo)
        {
            String invalidParameterInfo = "";
            if (!HomeControllerHelper.CheckChangePassengerInfo(changePassengerInfo, out invalidParameterInfo))
            {
                ViewBag.Message = invalidParameterInfo;
                return View(myTickets);
            }

            using var channel = GrpcChannel.ForAddress("https://localhost:7146");
            var client = new ChangePassenger.ChangePassengerClient(channel);

            ChangePassengerRequest changePassengerRequest = HomeControllerHelper.FromChangePassengerInfoToChangePassengerRequest(changePassengerInfo);
            var reply = client.ChangePassenger(changePassengerRequest);

            if (!HomeControllerHelper.CheckChangePassengerReply(reply, out invalidParameterInfo))
            {
                ViewBag.Message = invalidParameterInfo;
                return View(myTickets);
            }

            ViewBag.Message = invalidParameterInfo;
            return Redirect("/Home/Cart");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}