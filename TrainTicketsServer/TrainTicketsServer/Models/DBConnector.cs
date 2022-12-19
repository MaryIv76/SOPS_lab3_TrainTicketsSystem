using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using TrainTicketsServer.Models.Entities;
using TrainTicketsServer.Protos;

namespace TrainTicketsServer.Models
{
    public class DBConnector
    {
        ApplicationContext db;

        public DBConnector(ApplicationContext db)
        {
            this.db = db;
        }

        public bool checkLoginData(string login, string password, ref int userId)
        {
            bool validLoginData = false;
            List<User> user = db.usersDbSet.Where(user => user.login.Equals(login) && user.password.Equals(password)).ToList();
            if (user.Count() > 0)
            {
                validLoginData = true;
                userId = user[0].user_id;
            }
            return validLoginData;
        }

        public List<TrainTicketsServer.Models.Entities.Route> getAvailableTrains(string from, string to, DateOnly date)
        {
            string dateStr = date.ToString();
            IQueryable<TrainTicketsServer.Models.Entities.Route> suitableRoutes = db.routesDbSet.Where(route => route.from.Equals(from) && route.to.Equals(to));
            var availableTrains = suitableRoutes.Where(route =>
                db.tripsDbSet.Where(trip =>
                trip.route_id.Equals(route.route_id)
                && trip.date.Equals(dateStr)
                && !db.ticketsDbSet.Where(ticket => ticket.trip_id.Equals(trip.trip_id)).Any()).Any());

            return availableTrains.ToList();
        }

        public List<SeatInfo> getAvailableSeats(int routeId, DateOnly date)
        {
            string dateStr = date.ToString();
            var trips = db.tripsDbSet.Where(trip =>
                trip.route_id == routeId
                && trip.date.Equals(dateStr)
                && !db.ticketsDbSet.Where(ticket => ticket.trip_id.Equals(trip.trip_id)).Any());

            var seatsInfo = trips.Join(db.seatsDbSet,
                trip => trip.seat,
                seat => seat.seat_id,
                (trip, seat) => new SeatInfo
                {
                    seat_id = seat.seat_id,
                    seat_number = seat.seat_number,
                    price = trip.price,
                    seat_type = seat.type
                }
                );

            return seatsInfo.ToList();
        }

        public List<TicketInfo> getTicketsByUser(int userId)
        {
            var myTickets = db.ticketsDbSet.Where(ticket => ticket.user_id == userId);
            var myTicketsFullInfo = myTickets.Join(db.tripsDbSet,
                ticket => ticket.trip_id,
                trip => trip.trip_id,
                (ticket, trip) => new
                {
                    ticketNumber = ticket.number,
                    surname = ticket.surname,
                    firstname = ticket.firstname,
                    thirdname = ticket.thirdname,
                    date = trip.date,
                    price = trip.price,
                    seaId = trip.seat,
                    routeId = trip.route_id
                }).Join(db.routesDbSet,
                ticketInfo => ticketInfo.routeId,
                route => route.route_id,
                (ticketInfo, route) => new
                {
                    ticketNumber = ticketInfo.ticketNumber,
                    surname = ticketInfo.surname,
                    firstname = ticketInfo.firstname,
                    thirdname = ticketInfo.thirdname,
                    date = ticketInfo.date,
                    price = ticketInfo.price,
                    seatId = ticketInfo.seaId,
                    from = route.from,
                    to = route.to,
                    departureTime = route.departure_time,
                    arrivalTime = route.arrival_time
                }).Join(db.seatsDbSet,
                ticketInfo => ticketInfo.seatId,
                seat => seat.seat_id,
                (ticketInfo, seat) => new TicketInfo
                {
                    ticketNumber = ticketInfo.ticketNumber,
                    surname = ticketInfo.surname,
                    firstname = ticketInfo.firstname,
                    thirdname = ticketInfo.thirdname,
                    date = ticketInfo.date,
                    price = ticketInfo.price,
                    from = ticketInfo.from,
                    to = ticketInfo.to,
                    departureTime = ticketInfo.departureTime,
                    arrivalTime = ticketInfo.arrivalTime, 
                    seatNumber = seat.seat_number
                });
            return myTicketsFullInfo.ToList();
        }

        public bool deleteTicket(int ticketNumber)
        {
            var ticketToDelete = db.ticketsDbSet.Where(ticket => ticket.number == ticketNumber);
            if (ticketToDelete.Count() <=0)
            {
                return false;
            }
            db.ticketsDbSet.Remove(ticketToDelete.ToList()[0]);
            db.SaveChanges();
            return true;
        }

        public bool updateTicket(int ticketNumber, string newSurname, string newFirstname, string newThirdname)
        {
            var ticketToUpdate = db.ticketsDbSet.Where(ticket => ticket.number == ticketNumber);
            if (ticketToUpdate.Count() <= 0)
            {
                return false;
            }
            ticketToUpdate.First().surname = newSurname;
            ticketToUpdate.First().firstname = newFirstname;
            ticketToUpdate.First().thirdname = newThirdname;
            int n = db.SaveChanges();
            return true;
        }

        public bool insertTicket(int userId, int tripId, string surname, string firstname, string thirdname)
        {
            Entities.Ticket ticket = new Entities.Ticket();
            bool validUser = db.usersDbSet.Where(user => user.user_id == userId).Any();
            if (!validUser)
            {
                return false;
            }
            bool validTrip = db.tripsDbSet.Where(trip => trip.trip_id == tripId).Any();
            if (!validTrip)
            {
                return false;
            }
            if (!tripIsFree(tripId))
            {
                return false;
            }

            ticket.user_id = userId;
            ticket.trip_id = tripId;
            ticket.surname = surname;   
            ticket.firstname = firstname;
            ticket.thirdname = thirdname;
            db.ticketsDbSet.Add(ticket);
            db.SaveChanges();
            return true;
        }

        private bool tripIsFree(int tripId)
        {
            return !db.ticketsDbSet.Where(ticket => ticket.trip_id == tripId).Any();
        }
    }
}
