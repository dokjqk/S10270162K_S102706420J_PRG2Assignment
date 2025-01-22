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
    public class Airline
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string code;
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        private Dictionary<string, Flight> flights;

        public Dictionary<string, Flight> Flights
        {
            get { return flights; }
            set { flights = value; }
        }

        public Airline() { }
        public Airline(string name, string code)
        {
            Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
            Flights = flights;
            Name = name;
            Code = code;
        }

        public bool AddFlight(Flight flight)
        {
            if (Flights.ContainsKey(flight.FlightNumber))
            {
                return false;
            }
            else
            {
                flights.Add(code, flight);
                return true;
            }
        }

        public bool RemoveFlight(Flight flight)
        {
            if (Flights.ContainsKey(flight.FlightNumber))
            {
                return false;
            }
            else
            {
                flights.Remove(flight.FlightNumber);
                return true;
            }
        }

        public double CalculateFees()
        {
            double fees = 0;
            foreach (KeyValuePair<string, Flight> entry in flights)
            {
                fees += entry.Value.CalculateFees();
            }
            if (flights.Count > 5)
            {
                fees = fees * 0.97;
            }
            if (flights.Count >= 3)
            {
                int countThree = flights.Count / 3;
                double additionalDiscount = countThree * 50;
                fees -= additionalDiscount;
            }
            return fees;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Code: {Code}, Flight: {flights.Count}";
        }
    }

}
