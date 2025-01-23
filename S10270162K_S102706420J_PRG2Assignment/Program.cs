using S10270162K_S102706420J_PRG2Assignment;

//==========================================================
// Student Number : S10270162
// Student Name : Hendi Wong Jia Ming
// Partner Name : Ahmad Danial Azman
//==========================================================
//==========================================================
// Student Number : S102706420
// Student Name : Ahmad Danial Azman
// Partner Name : Hendi Wong Jia Ming
//==========================================================


// Basic Features
Terminal terminal5 = new Terminal("Terminal 5");

// 1. Load files (airlines and boarding gates) (Hendi)
static void LoadAirlinesAndBoardingGates(Terminal terminal5)
{
    const string airlineFile = "airlines.csv";
    const string boardingGatesFile = "boardinggates.csv";

    using (StreamReader fileReader = new StreamReader(airlineFile))
    {
        fileReader.ReadLine();
        string line;
        while((line = fileReader.ReadLine()) != null)
        {
            string[] fileArray = line.Split(',');
            Airline airline = new Airline(fileArray[0], fileArray[1]);
            terminal5.AddAirline(airline);
        }
    }

    using (StreamReader fileReader = new StreamReader(boardingGatesFile))
    {
        fileReader.ReadLine();
        string line;
        while ((line = fileReader.ReadLine()) != null)
        {
            string[] fileArray = line.Split(',');
            BoardingGate boardingGate = new BoardingGate(fileArray[0], bool.Parse(fileArray[1]), bool.Parse(fileArray[2]), bool.Parse(fileArray[3]));
            terminal5.AddBoardingGate(boardingGate);
        }
    }
}

LoadAirlinesAndBoardingGates(terminal5);


// Test code for Question 1
// Console.WriteLine(terminal5.ToString());

// 2. Load files (flights) (Ahmad)
static void LoadFlights(Terminal terminal5)
{
    const string flightsFile = "flights.csv";

    using (StreamReader fileReader = new StreamReader(flightsFile))
    {
        fileReader.ReadLine();
        string line;
        while ((line = fileReader.ReadLine()) != null)
        {
            string[] fileArray = line.Split(',');
            string flightNumber = fileArray[0];
            string origin = fileArray[1];
            string destination = fileArray[2];
            DateTime expectedTime = DateTime.Parse(fileArray[3]);
            string specialRequestCode = fileArray[4];

            Flight flight;
            //For Creating Different FLight objects based on special request code for future use possibly
            switch (specialRequestCode)
            {
                case "CFFT":
                    flight = new CFFTFlight(flightNumber, origin, destination, expectedTime, "Scheduled", 150);
                    break;
                case "DDJB":
                    flight = new DDJBFlight(flightNumber, origin, destination, expectedTime, "Scheduled", 300);
                    break;
                case "LWTT":
                    flight = new LWTTFlight(flightNumber, origin, destination, expectedTime, "Scheduled", 500);
                    break;
                default:
                    flight = new NORMFlight(flightNumber, origin, destination, expectedTime, "Scheduled");
                    break;
            }

            Airline airline = terminal5.airlines.Values.FirstOrDefault(a => flightNumber.StartsWith(a.Code));
            if (airline != null)
            {
                airline.AddFlight(flight);
                
            }
            else
            {
                Console.WriteLine($"No matching airline found for flight {flightNumber}");
            }
        }
    }
}
LoadFlights(terminal5);


// 3. List all flights with their basic information (Ahmad)
static void ListAllFlights(Terminal terminal5)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of All Flights");
    Console.WriteLine("=============================================");
    Console.WriteLine(String.Format("{0,-10} {1,-20} {2,-20} {3,-20} {4,-20}", "Flight No", "Airline Name", "Origin", "Destination", "Expected Time"));

    foreach (var airline in terminal5.airlines.Values)
    {
        foreach (var flight in airline.Flights.Values)
        {
            Console.WriteLine(String.Format("{0,-10} {1,-20} {2,-20} {3,-20} {4,-20}", flight.FlightNumber, airline.Name, flight.Origin, flight.Destination, flight.ExpectedTime));
        }
    }
}

ListAllFlights(terminal5);

// 4. List all boarding gates (Hendi)
static void ListAllBoardingGates(Terminal terminal5)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine(String.Format("{0,-15} {1,-10} {2,-10} {3,-10}", "Gate Name", "DDJB", "CFFT", "LWTT"));
    Dictionary<string, BoardingGate> airlineDict = terminal5.boardingGates;
    foreach (KeyValuePair<string, BoardingGate> entry in airlineDict)
    {
        Console.WriteLine(String.Format("{0,-15} {1,-10} {2,-10} {3,-10}", entry.Key, entry.Value.SupportsDDJB, entry.Value.SupportsCFFT, entry.Value.SupportsLWTT));
    }
}
ListAllBoardingGates(terminal5);


// 5. Assign a boarding gate to a flight (Ahmad)
// 6. Create a new flight (Ahmad)
// 7. Display full flight details from an airline (Hendi)
// 8. Modify flight details (Hendi)
// 9. Display scheduled flights in chronological order, with boarding gates assignments where applicable (Ahmad)
// Advanced Features

