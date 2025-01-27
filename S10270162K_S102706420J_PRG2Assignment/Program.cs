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
static void AssignBoardingGate(Terminal terminal5)
{
    Console.Write("Enter Flight Number: ");
    string flightNumber = Console.ReadLine();

    Flight flight = null;
    Airline airline = null;
    foreach (var a in terminal5.airlines.Values)
    {
        if (a.Flights.TryGetValue(flightNumber, out flight))
        {
            airline = a;
            break;
        }
    }

    if (flight == null)
    {
        Console.WriteLine("Flight not found.");
        return;
    }

    Console.WriteLine("Flight Information:");
    Console.WriteLine($"Flight Number: {flight.FlightNumber}");
    Console.WriteLine($"Airline: {airline.Name}");
    Console.WriteLine($"Origin: {flight.Origin}");
    Console.WriteLine($"Destination: {flight.Destination}");
    Console.WriteLine($"Expected Time: {flight.ExpectedTime}");
    Console.WriteLine($"Special Request Code: {flight.GetType().Name}");

    while (true)
    {
        Console.Write("Enter Boarding Gate: ");
        string gateName = Console.ReadLine();

        if (terminal5.boardingGates.TryGetValue(gateName, out BoardingGate gate))
        {
            if (gate.Flight == null)
            {
                gate.Flight = flight;
                Console.WriteLine("Boarding Gate Assignment:");
                Console.WriteLine($"Flight Number: {flight.FlightNumber}");
                Console.WriteLine($"Special Request Code: {flight.GetType().Name}");
                Console.WriteLine($"Boarding Gate: {gate.GateName}");


                Console.Write("Would you like to update the Status of the Flight? (Y/N): ");
                string updateStatus = Console.ReadLine().ToUpper();
                if (updateStatus == "Y")
                {
                    Console.Write("Enter new Status (Delayed/Boarding/On Time): ");
                    flight.Status = Console.ReadLine();
                }
                else
                {
                    flight.Status = "On Time";
                }

                Console.WriteLine("Boarding Gate assignment successful.");
                break;
            }
            else
            {
                Console.WriteLine("The Boarding Gate is already assigned to another flight. Please choose a different gate.");
            }
        }
        else
        {
            Console.WriteLine("Invalid Boarding Gate. Please try again.");
        }
    }
}
AssignBoardingGate(terminal5);
// 6. Create a new flight (Ahmad)
static void CreateNewFlight(Terminal terminal5)
{
    while (true)
    {
        Console.Write("Enter Flight Number: ");
        string flightNumber = Console.ReadLine();

        Console.Write("Enter Origin: ");
        string origin = Console.ReadLine();

        Console.Write("Enter Destination: ");
        string destination = Console.ReadLine();

        Console.Write("Enter Expected Departure/Arrival Time (e.g., 11:45 AM): ");
        DateTime expectedTime = DateTime.Parse(Console.ReadLine());

        Console.Write("Would you like to enter a Special Request Code? (Y/N): ");
        string specialRequest = Console.ReadLine().ToUpper();
        string specialRequestCode = null;
        if (specialRequest == "Y")
        {
            Console.Write("Enter Special Request Code: ");
            specialRequestCode = Console.ReadLine();
        }

        Flight flight;
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
            AppendFlightToFile(flight);
            Console.WriteLine("Flight added successfully.");
        }
        else
        {
            Console.WriteLine("No matching airline found for the flight.");
        }

        Console.Write("Would you like to add another Flight? (Y/N): ");
        string addAnother = Console.ReadLine().ToUpper();
        if (addAnother != "Y")
        {
            break;
        }
    }
}

static void AppendFlightToFile(Flight flight)
{
    const string flightsFile = "flights.csv";
    using (StreamWriter fileWriter = new StreamWriter(flightsFile, true))
    {
        fileWriter.WriteLine($"{flight.FlightNumber},{flight.Origin},{flight.Destination},{flight.ExpectedTime},{flight.GetType().Name}");
    }
}
//CreateNewFlight(terminal5);
// 7. Display full flight details from an airline (Hendi)
// 8. Modify flight details (Hendi)
// 9. Display scheduled flights in chronological order, with boarding gates assignments where applicable (Ahmad)
// Advanced Features

