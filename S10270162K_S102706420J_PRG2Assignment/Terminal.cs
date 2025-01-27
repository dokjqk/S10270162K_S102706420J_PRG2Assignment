//==========================================================
// Student Number : S10270642
// Student Name : Ahmad Danial Azman
// Partner Name : Hendi Wong Jia Ming
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace S10270162K_S102706420J_PRG2Assignment
{
    public class Terminal
    {
        //readonly used for immutability
        public readonly string terminalName;
        public readonly Dictionary<string, Airline> airlines = new Dictionary<string, Airline>();
        public readonly Dictionary<string, BoardingGate> boardingGates = new Dictionary<string, BoardingGate>();
        public readonly Dictionary<string, double> gateFees = new Dictionary<string, double>();

        public Terminal(string terminalName)
        {
            this.terminalName = terminalName;;
        }
        //Adding airline tot the terminal
        public bool AddAirline(Airline airline)
        {
            if (airline == null || airlines.ContainsKey(airline.Code))
            {
                return false;
            }
            airlines.Add(airline.Code, airline);
            return true;
        }
        //Adding boarding gate to terminal
        public bool AddBoardingGate(BoardingGate boardingGate)
        {
            if (boardingGate == null || boardingGates.ContainsKey(boardingGate.GateName))
            {
                return false;
            }
            boardingGates.Add(boardingGate.GateName, boardingGate);
            return true;
        }
        //Retrieves airline related to its respective flight 
        public Airline GetAirlineFromFlight(Flight flight)
        {
            if (flight == null)
            {
                return null;
            }

            foreach (var airline in airlines.Values)
            {
                if (airline.Flights.ContainsKey(flight.FlightNumber))
                {
                    return airline;
                }
            }
            return null; // No matching airline found
        }
        //Display fees for gate 
        public void PrintAirlineFees()
        {
            Console.WriteLine($"Gate Fees for Terminal: {terminalName}");
            foreach (var fee in gateFees)
            {
                Console.WriteLine($"Gate {fee.Key}: ${fee.Value:F2}");
            }
        }

        public override string ToString()
        {
            return $"Terminal: {terminalName}, Airlines: {airlines.Count}, Boarding Gates: {boardingGates.Count}";
        }
    }
}


