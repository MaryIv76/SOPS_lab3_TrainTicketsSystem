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
        List<TrainTicketsClient.Models.Ticket> myTickets;
        String invalidDeleteInfo;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            invalidDeleteInfo = "";

            /*myTickets = new List<TrainTicketsClient.Models.Ticket>();
            TrainTicketsClient.Models.Ticket ticket1 = new TrainTicketsClient.Models.Ticket();
            ticket1.ticketNumber = 1111;
            ticket1.surname = "Ivanova";
            ticket1.firstname = "Mary";
            ticket1.thirdname = "Dmitrievna";
            ticket1.from = "Grodno";
            ticket1.to = "Minsk";
            ticket1.departureTime = DateTime.Now.ToString("HH:mm");
            ticket1.arrivalTime = DateTime.Now.ToString("HH:mm");
            ticket1.date = DateTime.Now.ToString("dd.MM.yyyy");
            ticket1.seatNumber = 5;
            ticket1.price = 50.0;
            myTickets.Add(ticket1);
            myTickets.Add(ticket1);*/
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

            using var channel = GrpcChannel.ForAddress("https://localhost:7009");
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

            using var channel = GrpcChannel.ForAddress("https://localhost:7009");
            var client = new TrainsInfo.TrainsInfoClient(channel);

            TrainsRequest trainsRequest = HomeControllerHelper.FromFindTrainToTrainsRequest(findTrain);
            var reply = client.GetAvailableTrains(trainsRequest);

            if (!HomeControllerHelper.CheckTrainsReply(reply, out invalidParameterInfo))
            {
                ViewBag.Message = invalidParameterInfo;
                return View();
            }
            List<RouteClass> routes = HomeControllerHelper.FromTrainsReplyToRouteClassList(reply, (DateTime)findTrain.date);

            /*List<RouteClass> routes = new List<RouteClass>();
            RouteClass route1 = new RouteClass();
            route1.routeId = 1;
            route1.arrivalTime = DateTime.Now.ToString("HH:mm");
            route1.departureTime = DateTime.Now.ToString("HH:mm");
            routes.Add(route1);
            routes.Add(route1);
            routes.Add(route1);*/

            ViewBag.Message = invalidParameterInfo;
            return View(routes);
        }

        public IActionResult ChooseSeat(int id, DateTime date)
        {
            String invalidParameterInfo = "";
            using var channel = GrpcChannel.ForAddress("https://localhost:7009");
            var client = new SeatsInfo.SeatsInfoClient(channel);

            SeatsRequest seatsRequest = HomeControllerHelper.FromIdToSeatsRequest(id, date);
            var reply = client.GetAvailableSeats(seatsRequest);

            if (!HomeControllerHelper.CheckSeatsReply(reply, out invalidParameterInfo))
            {
                ViewBag.Message = invalidParameterInfo;
                return View();
            }
            List<TrainTicketsClient.Models.Seat> seats = HomeControllerHelper.FromSeatsReplyToSeatList(reply);

            /*List<TrainTicketsClient.Models.Seat> seats = new List<TrainTicketsClient.Models.Seat>();
            TrainTicketsClient.Models.Seat seat1 = new TrainTicketsClient.Models.Seat();
            seat1.id = 1;
            seat1.seatNumber = 1;
            seat1.type = "type";
            seat1.price = 15.0;
            seats.Add(seat1);
            seats.Add(seat1);*/

            ViewBag.Message = invalidParameterInfo;
            return View(seats);
        }

        public IActionResult BuyTicket(int id, int seatNumber, string type, double price)
        {
            ViewBag.SeatId = id;
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
                ViewBag.SeatId = passenger.seatId;
                ViewBag.SeatNumber = passenger.seatNumber;
                ViewBag.Type = passenger.type;
                ViewBag.Price = passenger.price;
                ViewBag.Message = invalidParameterInfo;
                return View();
            }

            using var channel = GrpcChannel.ForAddress("https://localhost:7009");
            var client = new BuyTicket.BuyTicketClient(channel);

            BuyTicketRequest buyTicketRequest = HomeControllerHelper.FromPassengerToBuyTicketRequest(passenger, userId);
            var reply = client.BuyTicket(buyTicketRequest);

            if (!HomeControllerHelper.CheckBuyTicketReply(reply, out invalidParameterInfo))
            {
                ViewBag.Message = invalidParameterInfo;
                return View();
            }

            return Redirect("/Home/Cart");
        }

        public IActionResult Cart()
        {
            String invalidParameterInfo = "";

            using var channel = GrpcChannel.ForAddress("https://localhost:7009");
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

            using var channel = GrpcChannel.ForAddress("https://localhost:7009");
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

            using var channel = GrpcChannel.ForAddress("https://localhost:7009");
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