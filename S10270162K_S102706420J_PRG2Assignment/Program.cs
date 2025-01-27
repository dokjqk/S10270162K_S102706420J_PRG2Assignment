using System.ComponentModel.DataAnnotations;
using S10270162K_S102706420J_PRG2Assignment;

//==========================================================
// Student Number : S10270162
// Student Name : Hendi Wong Jia Ming
// Partner Name : Ahmad Danial Azman
//==========================================================
//==========================================================
// Student Number : S10270642
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
            //For Creating Different Flight objects based on special request code for future use possibly
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
static Airline DisplayFullFlightDetails(Terminal terminal5)
{
    Airline airlineSearch = null;
    Console.WriteLine("=============================================");
    Console.WriteLine($"List of Airlines for Changi Airport {terminal5.terminalName}");
    Console.WriteLine("=============================================");
    Console.WriteLine(String.Format("{0,-15} {1,-10}", "Airline Code", "Airline Name"));
    Dictionary<string, Airline> airlineDict = terminal5.airlines;
    foreach (KeyValuePair<string, Airline> entry in airlineDict)
    {
        Console.WriteLine(String.Format("{0,-15} {1,-10}", entry.Key, entry.Value.Name));
    }
    Console.Write("Enter Airline Code: ");
    string airlineCode = Console.ReadLine();
    foreach (KeyValuePair<string, Airline> entry in airlineDict)
    {
        if (entry.Key == airlineCode)
        {
            airlineSearch = entry.Value;
        }
    }
    Console.WriteLine("=============================================");
    Console.WriteLine($"List of Flights for {airlineSearch.Name}");
    Console.WriteLine("=============================================");
    Console.WriteLine(String.Format("{0,-15} {1,-30} {2,-15} {3,-20} {4,-35} {5,-20} {6,-20}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time", "Status", "Special Request Code", "Boarding Gate"));

    foreach (KeyValuePair<string, Flight> entry in airlineSearch.Flights)
    {
        string specialRequestCode = null;
        if (entry.Value is CFFTFlight)
        {
            specialRequestCode = "CFFT";
        }
        else if (entry.Value is DDJBFlight)
        {
            specialRequestCode = "DDJB";
        }
        else if (entry.Value is LWTTFlight)
        {
            specialRequestCode = "LWTT";
        }
        else if (entry.Value is NORMFlight)
        {
            specialRequestCode = "NORM";
        }
        Console.WriteLine(String.Format("{0,-15} {1,-30} {2,-15} {3,-20} {4,-35} {5,-20} {6,-20}", entry.Key, airlineSearch.Name, entry.Value.Origin, entry.Value.Destination, entry.Value.ExpectedTime, entry.Value.Status, specialRequestCode));
    }
    return airlineSearch;
}
DisplayFullFlightDetails(terminal5);
// 8. Modify flight details (Hendi)
static void ModifyFlightDetails(Terminal terminal5)
{
    Airline airlineSearch = DisplayFullFlightDetails(terminal5);
    Flight flightObjectToModify = null;
    while (true)
    {
        Console.Write("Choose an existing Flight to modify or delete: ");
        string flightToModify = Console.ReadLine();
        bool flightFound = false;
        foreach (KeyValuePair<string, Flight> entry in airlineSearch.Flights)
        {
            if (entry.Key == flightToModify)
            {
                flightFound = true;
                flightObjectToModify = entry.Value;
                break;
            }
        }
        if (!flightFound)
        {
            Console.WriteLine("Flight not found");
            continue;
        }
        else
        {
            break;
        }
    }
    Console.WriteLine(flightObjectToModify);
    Console.WriteLine("1. Modify Flight");
    Console.WriteLine("2. Delete Flight");
    Console.Write("Choose an option: ");
    string optionToModify = Console.ReadLine();
    if (optionToModify == "1")
    {
        Console.WriteLine("1. Modify Basic Information");
        Console.WriteLine("2. Modify Status");
        Console.WriteLine("3. Modify Special Request Code");
        Console.WriteLine("4. Modify Boarding Gate");
        Console.Write("Choose an option: ");
        string modifyOptions = Console.ReadLine();
        if (modifyOptions == "1")
        {
            Console.Write("Enter new Origin: ");
            string newOrigin = Console.ReadLine();
            Console.Write("Enter new Destination: ");
            string newDestination = Console.ReadLine();
            Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
            DateTime newDateTime = DateTime.Parse(Console.ReadLine());
            Console.WriteLine($"{newOrigin} {newDestination} {newDateTime}");
            flightObjectToModify.Origin = newOrigin;
            flightObjectToModify.Destination = newDestination;
            flightObjectToModify.ExpectedTime = newDateTime;
            Console.WriteLine("Flight updated!");
        }
        else if (modifyOptions == "2")
        {
            Console.WriteLine($"Flight Number: {flightObjectToModify.FlightNumber}");
            Console.WriteLine($"Airline Name: {airlineSearch.Name}");
            Console.WriteLine($"Flight Origin: {flightObjectToModify.Origin}");
            Console.WriteLine($"Flight Destination: {flightObjectToModify.Destination}");
            Console.Write("Enter new Status: ");
            string statusToModify = Console.ReadLine();
            flightObjectToModify.Status = statusToModify;


        }
        else if (modifyOptions == "3")
        {
            Console.WriteLine($"Flight Number: {flightObjectToModify.FlightNumber}");
            Console.WriteLine($"Airline Name: {airlineSearch.Name}");
            Console.WriteLine($"Flight Origin: {flightObjectToModify.Origin}");
            Console.WriteLine($"Flight Destination: {flightObjectToModify.Destination}");
            Console.Write("Enter new Special Request Code: ");
            string codeToModify = Console.ReadLine();
            // 6. Create a new flight (Ahmad)
        }
        else
        {
            Console.WriteLine($"Flight Number: {flightObjectToModify.FlightNumber}");
            Console.WriteLine($"Airline Name: {airlineSearch.Name}");
            Console.WriteLine($"Flight Origin: {flightObjectToModify.Origin}");
            Console.WriteLine($"Flight Destination: {flightObjectToModify.Destination}");
            Console.Write("Enter new Boarding Gate: ");
            string gateToModify = Console.ReadLine();
            // 5. Assign a boarding gate to a flight (Ahmad)
        }
    }
    else
    {
        bool result = airlineSearch.RemoveFlight(flightObjectToModify);
        Console.WriteLine("Flight removed");
    }
    DisplayFullFlightDetails(terminal5);
}
ModifyFlightDetails(terminal5);

// 9. Display scheduled flights in chronological order, with boarding gates assignments where applicable (Ahmad)
// Advanced Features

