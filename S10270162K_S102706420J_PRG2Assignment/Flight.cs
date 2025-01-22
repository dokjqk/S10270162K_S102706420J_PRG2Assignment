//==========================================================
// Student Number : S10270162
// Student Name : Hendi Wong Jia Ming
// Partner Name : Ahmad Danial Azman
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10270162K_S102706420J_PRG2Assignment
{
    public abstract class Flight
    {
        private string flightNumber;
        public string FlightNumber
        { 
            get {  return flightNumber; } 
            set { flightNumber = value; } 
        }

        public string origin;
        public string Origin
        { 
            get { return origin; } 
            set { origin = value; } 
        }

        public string destination;
        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        private DateTime expectedTime;

        public DateTime ExpectedTime
        {
            get { return expectedTime; }
            set { expectedTime = value; }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public Flight() { }

        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
        }

        public abstract double CalculateFees();

        public override string ToString()
        {
            return $"flightNumber: {flightNumber}, origin: {origin}, destination: {destination}, expectedTime: {expectedTime}, status {status}";
        }
    }
}
