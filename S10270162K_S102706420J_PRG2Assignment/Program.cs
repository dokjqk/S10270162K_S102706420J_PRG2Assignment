using System.ComponentModel.DataAnnotations;
using System.Text;
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

Terminal terminal5 = new Terminal("Terminal 5");
LoadAirlinesAndBoardingGates(terminal5);
LoadFlights(terminal5);
double numberOfAirlines = terminal5.airlines.Count;
double numberOfBoardingGate = terminal5.boardingGates.Count;
double numberOfFlight = 0;
foreach (KeyValuePair<string, Airline> entry in terminal5.airlines)
{
    numberOfFlight += entry.Value.Flights.Count;
}

Console.WriteLine("Loading Airlines...");
Console.WriteLine($"{numberOfAirlines} Airlines Loaded!");
Console.WriteLine("Loading Boarding Gates...");
Console.WriteLine($"{numberOfBoardingGate} Boarding Gates Loaded!");
Console.WriteLine("Loading Flights...");
Console.WriteLine($"{numberOfFlight} Flights Loaded!");

while (true)
{
    DisplayMenu();
    string option = Console.ReadLine();
    int parsedOption;

    if (string.IsNullOrEmpty(option) || !int.TryParse(option, out parsedOption))
    {
        Console.WriteLine("Invalid input. Please try again.");
        continue;
    }
    if (parsedOption < 0 || parsedOption > 9)
    {
        Console.WriteLine("Invalid option. Please enter a number between 0 and 9.");
        continue;
    }
    switch (parsedOption)
    {
        case 0:
            Console.WriteLine("Goodbye!");
            return;
        case 1:
            ListAllFlights(terminal5);
            break;
        case 2:
            ListAllBoardingGates(terminal5);
            break;
        case 3:
            AssignBoardingGate(terminal5);
            break;
        case 4:
            CreateNewFlight(terminal5);
            break;
        case 5:
            DisplayFullFlightDetails(terminal5);
            break;
        case 6:
            ModifyFlightDetails(terminal5);
            break;
        case 7:
            DisplayScheduledFlights(terminal5);
            break;
        case 8:
            ProcessUnassignedFlights(terminal5);
            break;
        case 9:
            DisplayTotalFeePerAirline(terminal5);
            break;
    }
}


// Display Menu
static void DisplayMenu()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Welcome to Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("3. Assign a Boarding Gate to a Flight");
    Console.WriteLine("4. Create Flight");
    Console.WriteLine("5. Display Airline Flights");
    Console.WriteLine("6. Modify Flight Details");
    Console.WriteLine("7. Display Flight Schedule");
    Console.WriteLine("8. Process all unassigned fights to boarding gates in bulk");
    Console.WriteLine("9. Display the total fee per airline for the day");
    Console.WriteLine("0. Exit");
    Console.WriteLine();
    Console.Write("Please select your option: ");
}


// Basic Features
// 1. Load files (airlines and boarding gates) (Hendi)
static void LoadAirlinesAndBoardingGates(Terminal terminal5)
{
    const string airlineFile = "airlines.csv";
    const string boardingGatesFile = "boardinggates.csv";

    // Check if the files exists.
    if (!File.Exists(airlineFile) || !File.Exists(boardingGatesFile))
    {
        Console.WriteLine("Error: One or more required files are missing.");
        return;
    }

    using (StreamReader fileReader = new StreamReader(airlineFile))
    {
        fileReader.ReadLine(); // Skip header.
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

// 2. Load files (flights) (Ahmad)
static void LoadFlights(Terminal terminal5)
{
    const string flightsFile = "flights.csv";

    if (!File.Exists(flightsFile))
    {
        Console.WriteLine($"Error: {flightsFile} not found.");
        return;
    }

    using (StreamReader fileReader = new StreamReader(flightsFile))
    {
        fileReader.ReadLine(); // Skip header.
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

// 3. List all flights with their basic information (Ahmad)
static void ListAllFlights(Terminal terminal5)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine(String.Format("{0,-15} {1,-20} {2,-20} {3,-20} {4,-35}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Date "));
    Console.WriteLine("Departure/Arrival Time");

    foreach (var airline in terminal5.airlines.Values)
    {
        foreach (var flight in airline.Flights.Values)
        {
            Console.WriteLine(String.Format("{0,-15} {1,-20} {2,-20} {3,-20} {4,-35}", flight.FlightNumber, airline.Name, flight.Origin, flight.Destination, flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm:ss tt")));
        }
    }
}

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

// 5. Assign a boarding gate to a flight (Ahmad)
static void AssignBoardingGate(Terminal terminal5)
{
    Flight flight = null;
    Airline airline = null;
    string flightNumber = null;

    while (true)
    {
        Console.Write("Enter Flight Number: ");
        flightNumber = Console.ReadLine();
        flightNumber = flightNumber.ToUpper();
        if (string.IsNullOrWhiteSpace(flightNumber))
        {
            Console.WriteLine("Flight number cannot be empty.");
            continue;
        }
        else
        {
            break;
        }
    }

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
        gateName = gateName.ToUpper().Trim().ToUpper();

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
                string updateStatus = Console.ReadLine().Trim().ToUpper();
                if (updateStatus == "Y")
                {
                    while (true)
                    {
                        Console.Write("Enter new Status (Delayed/Boarding/On Time): ");
                        string status = Console.ReadLine();
                        if (status == "Delayed" || status == "Boarding" || status == "On Time")
                        {
                            flight.Status = status;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid status. Please enter 'Delayed', 'Boarding', or 'On Time'.");
                        }
                    }
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
// 6. Create a new flight (Ahmad)
static void CreateNewFlight(Terminal terminal5)
{
    List<string> destinationList = new List<string> { "Singapore (SIN)", "Tokyo (NRT)", "Manila (MNL)", "Sydney (SYD)", "Kuala Lumpur (KUL)", "Jakarta (CGK)", "Dubai (DXB)", "Melbourne (MEL)", "London (LHR)", "Hong Kong (HKD)", "Bangkok (BKK)", "Melbourne (MEL)", "Guangzhou (CAN)", "Frankfurt (FRA)" };
    while (true)
    {
        string flightNumber;
        while (true)
        {
            Console.Write("Enter Flight Number: ");
            flightNumber = Console.ReadLine();
            if (!string.IsNullOrEmpty(flightNumber))
            {
                break;
            }
            Console.WriteLine("Flight Number cannot be empty. Please try again.");
        }

        string origin;
        while (true)
        {
            Console.Write("Enter Origin: ");
            origin = Console.ReadLine();
            if (!string.IsNullOrEmpty(origin) && destinationList.Contains(origin))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid origin.");
            }
        }

        string destination;
        while (true)
        {
            Console.Write("Enter Destination: ");
            destination = Console.ReadLine();
            if (!string.IsNullOrEmpty(destination) && destinationList.Contains(destination))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid destination.");
            }
        }

        DateTime expectedTime;
        while (true)
        {
            Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
            if (DateTime.TryParse(Console.ReadLine(), out expectedTime))
            {
                break;
            }
            Console.WriteLine("Invalid date format. Please try again.");
        }

        Console.Write("Would you like to enter a Special Request Code? (Y/N): ");
        string specialRequest = Console.ReadLine().ToUpper();
        string specialRequestCode = null;
        if (specialRequest == "Y")
        {
            while (true)
            {
                Console.Write("Enter Special Request Code: ");
                specialRequestCode = Console.ReadLine();
                if (specialRequestCode == "CFFT" || specialRequestCode == "DDJB" || specialRequestCode == "LWTT" || string.IsNullOrEmpty(specialRequestCode))
                {
                    break;
                }
                Console.WriteLine("Invalid Special Request Code. Please enter 'CFFT', 'DDJB', 'LWTT', or leave it empty.");
            }
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
    if (!File.Exists(flightsFile))
    {
        Console.WriteLine($"Error: {flightsFile} not found.");
        return;
    }
    using (StreamWriter fileWriter = new StreamWriter(flightsFile, true))
    {
        fileWriter.WriteLine($"{flight.FlightNumber},{flight.Origin},{flight.Destination},{flight.ExpectedTime},{flight.GetType().Name}");
    }
}
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
    while (true) // Loop until a valid airline code is entered
    {
        Console.Write("Enter Airline Code: ");
        string airlineCode = Console.ReadLine();

        foreach (KeyValuePair<string, Airline> entry in airlineDict)
        {
            if (entry.Key == airlineCode)
            {
                airlineSearch = entry.Value;
                break;
            }
        }
        if (airlineSearch != null)
        {
            break;
        }
        else
        {
            Console.WriteLine("Airline not found. Please try again.");
        }
    }
    Console.WriteLine("=============================================");
    Console.WriteLine($"List of Flights for {airlineSearch.Name}");
    Console.WriteLine("=============================================");
    Console.WriteLine(String.Format("{0,-20} {1,-35} {2,-20} {3,-25} {4,-40} {5,-25} {6,-25} {7,-25}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time", "Status", "Special Request Code", "Boarding Gate"));

    foreach (KeyValuePair<string, Flight> entry in airlineSearch.Flights)
    {
        string specialRequestCode = null;
        string boardingGate = null;
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
        foreach (KeyValuePair<string, BoardingGate> entryBoarding in terminal5.boardingGates)
        {
            if (entryBoarding.Value.Flight == entry.Value)
            {
                boardingGate = entryBoarding.Key;
            }
        }
        Console.WriteLine(String.Format("{0,-20} {1,-35} {2,-20} {3,-25} {4,-40} {5,-25} {6,-25} {7,-25}", entry.Key, airlineSearch.Name, entry.Value.Origin, entry.Value.Destination, entry.Value.ExpectedTime, entry.Value.Status, specialRequestCode, boardingGate));
    }
    return airlineSearch;
}
// 8. Modify flight details (Hendi)
static void ModifyFlightDetails(Terminal terminal5)
{
    List<string> destinationList = new List<string> { "Singapore (SIN)", "Tokyo (NRT)", "Manila (MNL)", "Sydney (SYD)", "Kuala Lumpur (KUL)", "Jakarta (CGK)", "Dubai (DXB)", "Melbourne (MEL)", "London (LHR)", "Hong Kong (HKD)", "Bangkok (BKK)", "Melbourne (MEL)", "Guangzhou (CAN)", "Frankfurt (FRA)" };
    Airline airlineSearch = DisplayFullFlightDetails(terminal5);
    Flight flightObjectToModify = null;
    while (true)
    {
        Console.Write("Choose an existing Flight to modify or delete: ");
        string flightToModify = Console.ReadLine().ToUpper().Trim();
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
    string optionToModify;
    while (true)
    {
        Console.WriteLine("1. Modify Flight");
        Console.WriteLine("2. Delete Flight");
        Console.Write("Choose an option: ");
        optionToModify = Console.ReadLine();
        if (string.IsNullOrEmpty(optionToModify) || (optionToModify != "1" && optionToModify != "2"))
        {
            Console.WriteLine("Please enter a number.");
            continue;
        }
        else
        {
            break;
        }
    }
    if (optionToModify == "1")
    {
        string modifyOptions;
        while (true)
        {
            Console.WriteLine("1. Modify Basic Information");
            Console.WriteLine("2. Modify Status");
            Console.WriteLine("3. Modify Special Request Code");
            Console.WriteLine("4. Modify Boarding Gate");
            Console.Write("Choose an option: ");
            modifyOptions = Console.ReadLine();

            if (string.IsNullOrEmpty(modifyOptions) || (modifyOptions != "1" && modifyOptions != "2" && modifyOptions != "3" && modifyOptions != "4"))
            {
                Console.WriteLine("Please enter a valid number.");
                continue;
            }
            else
            {
                break;
            }

        }
        if (modifyOptions == "1")
        {
            string newOrigin;
            while (true)
            {
                Console.Write("Enter new Origin: ");
                newOrigin = Console.ReadLine();
                if (!string.IsNullOrEmpty(newOrigin) && destinationList.Contains(newOrigin))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid origin.");
                }
            }

            string newDestination;
            while (true)
            {
                Console.Write("Enter new Destination: ");
                newDestination = Console.ReadLine();
                if (!string.IsNullOrEmpty(newDestination) && destinationList.Contains(newDestination))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid destination.");
                }
            }


            DateTime newDateTime;
            while (true)
            {
                Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                if (DateTime.TryParse(Console.ReadLine(), out newDateTime))
                {
                    break;
                }
                Console.WriteLine("Invalid date format. Please try again.");
            }

            Console.WriteLine($"{newOrigin} {newDestination} {newDateTime}");
            flightObjectToModify.Origin = newOrigin;
            flightObjectToModify.Destination = newDestination;
            flightObjectToModify.ExpectedTime = newDateTime;
            Console.WriteLine("Flight updated!");
            Console.WriteLine($"Flight Number: {flightObjectToModify.FlightNumber}");
            Console.WriteLine($"Airline Name: {airlineSearch.Name}");
            Console.WriteLine($"Flight Origin: {flightObjectToModify.Origin}");
            Console.WriteLine($"Flight Destination: {flightObjectToModify.Destination}");
            Console.WriteLine($"Flight Expected Time: {flightObjectToModify.Destination}");
            Console.WriteLine($"Status: {flightObjectToModify.Status}");
        }
        else if (modifyOptions == "2")
        {
            Console.WriteLine($"Flight Number: {flightObjectToModify.FlightNumber}");
            Console.WriteLine($"Airline Name: {airlineSearch.Name}");
            Console.WriteLine($"Flight Origin: {flightObjectToModify.Origin}");
            Console.WriteLine($"Flight Destination: {flightObjectToModify.Destination}");
            Console.WriteLine($"Flight Expected Time: {flightObjectToModify.Destination}");
            string statusToModify;

            while (true)
            {
                Console.Write("Enter new Status: ");
                statusToModify = Console.ReadLine();
                if (statusToModify == "Delayed" || statusToModify == "Boarding" || statusToModify == "On Time")
                {
                    flightObjectToModify.Status = statusToModify;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid status. Please enter 'Delayed', 'Boarding', or 'On Time'.");
                }
            }

        }
        else if (modifyOptions == "3")
        {
            Console.WriteLine($"Flight Number: {flightObjectToModify.FlightNumber}");
            string flightNumber = flightObjectToModify.FlightNumber;
            Console.WriteLine($"Airline Name: {airlineSearch.Name}");
            string airlineName = airlineSearch.Name;
            Console.WriteLine($"Flight Origin: {flightObjectToModify.Origin}");
            string origin = flightObjectToModify.Origin;
            Console.WriteLine($"Flight Destination: {flightObjectToModify.Destination}");
            string destination = flightObjectToModify.Destination;
            Console.WriteLine($"Flight Expected Time: {flightObjectToModify.ExpectedTime}");
            DateTime expectedTime = flightObjectToModify.ExpectedTime;
            string codeToModify;

            while (true)
            {
                Console.Write("Enter new Special Request Code: ");
                codeToModify = Console.ReadLine();
                if (codeToModify == "CFFT" || codeToModify == "DDJB" || codeToModify == "LWTT" || string.IsNullOrEmpty(codeToModify))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Special Request Code");
                }
            }

            Flight flight;
            switch (codeToModify)
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
            airlineSearch.RemoveFlight(flightObjectToModify);
            airlineSearch.AddFlight(flight);
        }
        else
        {
            while (true)
            {
                Console.WriteLine($"Flight Number: {flightObjectToModify.FlightNumber}");
                Console.WriteLine($"Airline Name: {airlineSearch.Name}");
                Console.WriteLine($"Flight Origin: {flightObjectToModify.Origin}");
                Console.WriteLine($"Flight Destination: {flightObjectToModify.Destination}");
                Console.Write("Enter new Boarding Gate: ");
                string gateToModify = Console.ReadLine().ToUpper().Trim();

                BoardingGate oldGate = null;
                BoardingGate newGate = null;
                bool flightFound = false;

                foreach (KeyValuePair<string, BoardingGate> entryBoarding in terminal5.boardingGates)
                {
                    if (entryBoarding.Value.Flight == flightObjectToModify)
                    {
                        oldGate = entryBoarding.Value;
                        flightFound = true;
                        break;
                    }
                }
                if (!flightFound)
                {
                    Console.WriteLine("Flight does not have a Boarding Gate");
                    Console.Write("Do you want to assign a Boarding Gate instead? (Y/N): ");
                    string option = Console.ReadLine();
                    if (option == "Y")
                    {
                        AssignBoardingGate(terminal5);
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                oldGate.Flight = null;
                Console.WriteLine($"Flight removed from {oldGate.GateName}.");
                foreach (KeyValuePair<string, BoardingGate> entryBoarding in terminal5.boardingGates)
                {
                    if (entryBoarding.Value.GateName == gateToModify)
                    {
                        newGate = entryBoarding.Value;
                        break;
                    }
                }
                newGate.Flight = flightObjectToModify;
                break;
            }
        }
    }
    else
    {
        bool result = airlineSearch.RemoveFlight(flightObjectToModify);
        Console.WriteLine("Flight removed");
    }
}

// 9. Display scheduled flights in chronological order, with boarding gates assignments where applicable (Ahmad)
static void DisplayScheduledFlights(Terminal terminal5)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Scheduled Flights in Chronological Order");
    Console.WriteLine("=============================================");
    Console.WriteLine(String.Format("{0,-10} {1,-20} {2,-20} {3,-20} {4,-25} {5,-15} {6,-15}", "Flight No", "Airline Name", "Origin", "Destination", "Expected Time", "Status", "Boarding Gate"));

    if (terminal5?.airlines == null) return;

    var allFlights = new List<Flight>();

    foreach (var airline in terminal5.airlines.Values)
    {
        allFlights.AddRange(airline.Flights.Values);
    }

    var sortedFlights = allFlights.OrderBy(f => f.ExpectedTime).ToList();

    foreach (var flight in sortedFlights)
    {
        string boardingGate = terminal5.boardingGates.Values.FirstOrDefault(g => g.Flight == flight)?.GateName ?? "None";
        Airline airline = terminal5.GetAirlineFromFlight(flight);
        Console.WriteLine(String.Format("{0,-10} {1,-20} {2,-20} {3,-20} {4,-25} {5,-15} {6,-15}", flight.FlightNumber, airline.Name, flight.Origin, flight.Destination, flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm:ss tt"), flight.Status, boardingGate));
    }
}

// Advanced Features
// Advanced Features: Process all unassigned flights to boarding gates in bulk
static void ProcessUnassignedFlights(Terminal terminal5)
{
    var unassignedFlights = new Queue<Flight>();
    var unassignedGates = new List<BoardingGate>();

    // Collect all unassigned flights
    foreach (var airline in terminal5.airlines.Values)
    {
        foreach (var flight in airline.Flights.Values)
        {
            if (!terminal5.boardingGates.Values.Any(g => g.Flight == flight))
            {
                unassignedFlights.Enqueue(flight);
            }
        }
    }

    // Collect all unassigned boarding gates
    foreach (var gate in terminal5.boardingGates.Values)
    {
        if (gate.Flight == null)
        {
            unassignedGates.Add(gate);
        }
    }

    int totalProcessedFlights = 0;
    int totalProcessedGates = 0;
    var assignedFlights = new List<(Flight, BoardingGate)>();

    // Process each unassigned flight
    while (unassignedFlights.Count > 0)
    {
        var flight = unassignedFlights.Dequeue();
        BoardingGate assignedGate = null;

        // Check if the flight has a special request code
        if (flight is CFFTFlight || flight is DDJBFlight || flight is LWTTFlight)
        {
            // Search for an unassigned boarding gate that matches the special request code
            assignedGate = unassignedGates.FirstOrDefault(g =>
                (flight is CFFTFlight && g.SupportsCFFT) ||
                (flight is DDJBFlight && g.SupportsDDJB) ||
                (flight is LWTTFlight && g.SupportsLWTT));
        }
        else
        {
            // Search for an unassigned boarding gate that has no special request code
            assignedGate = unassignedGates.FirstOrDefault(g => !g.SupportsCFFT && !g.SupportsDDJB && !g.SupportsLWTT);
        }

        if (assignedGate != null)
        {
            assignedGate.Flight = flight;
            unassignedGates.Remove(assignedGate);
            totalProcessedFlights++;
            totalProcessedGates++;
            assignedFlights.Add((flight, assignedGate));
        }
    }

    // Display results
    Console.WriteLine("=============================================");
    Console.WriteLine("Processing Results");
    Console.WriteLine("=============================================");
    Console.WriteLine($"Total number of unassigned flights: {unassignedFlights.Count}");
    Console.WriteLine($"Total number of unassigned boarding gates: {unassignedGates.Count}");
    Console.WriteLine($"Total number of flights processed and assigned: {totalProcessedFlights}");
    Console.WriteLine($"Total number of boarding gates processed and assigned: {totalProcessedGates}");

    // Display the percentage of flights and boarding gates that were processed automatically
    double totalFlights = terminal5.airlines.Values.Sum(a => a.Flights.Count);
    double totalGates = terminal5.boardingGates.Count;
    double percentageFlightsProcessed = totalFlights > 0 ? (totalProcessedFlights / totalFlights) * 100 : 0;
    double percentageGatesProcessed = totalGates > 0 ? (totalProcessedGates / totalGates) * 100 : 0;
    Console.WriteLine($"Percentage of flights processed automatically: {percentageFlightsProcessed:F2}%");
    Console.WriteLine($"Percentage of boarding gates processed automatically: {percentageGatesProcessed:F2}%");

    // Display assigned flight details
    Console.WriteLine("=============================================");
    Console.WriteLine("Assigned Flight Details");
    Console.WriteLine("=============================================");
    foreach (var (flight, gate) in assignedFlights)
    {
        Airline airline = terminal5.GetAirlineFromFlight(flight);
        Console.WriteLine($"Flight Number: {flight.FlightNumber}");
        Console.WriteLine($"Airline Name: {airline.Name}");
        Console.WriteLine($"Origin: {flight.Origin}");
        Console.WriteLine($"Destination: {flight.Destination}");
        Console.WriteLine($"Expected Time: {flight.ExpectedTime}");
        Console.WriteLine($"Special Request Code: {flight.GetType().Name}");
        Console.WriteLine($"Boarding Gate: {gate.GateName}");
        Console.WriteLine("---------------------------------------------");
    }
}

// Advanced Features: Display the total fee per airline for the day
static void DisplayTotalFeePerAirline(Terminal terminal5)
{
    bool unassignedFound = false;
    // Find unassigned flights
    foreach (KeyValuePair<string, Airline> entry in terminal5.airlines)
    {
        Airline airlineTerminal = entry.Value;
        foreach (KeyValuePair<string, Flight> entryFlight in airlineTerminal.Flights)
        {
            bool matchFound = false;
            Flight airlineFlight = entryFlight.Value;
            foreach (KeyValuePair<string, BoardingGate> entryBoardingGate in terminal5.boardingGates)
            {
                BoardingGate terminalBoardingGate = entryBoardingGate.Value;
                if (airlineFlight == terminalBoardingGate.Flight)
                {
                    matchFound = true;
                }
            }
            if (matchFound == false)
            {
                unassignedFound = true;
            }
        }
    }
    if (unassignedFound != false)
    {
        Console.WriteLine("Ensure that all unassigned Flights have their Boarding Gates assigned before running this feature again.");
    }
    else
    {
        Console.WriteLine("All flight assigned.");
        foreach (KeyValuePair<string, Airline> entry in terminal5.airlines)
        {
            Airline airlineTerminal = entry.Value;
            double totalOriginalFees = 0;
            double totalFlightDiscount = 0;
            foreach (KeyValuePair<string, Flight> entryFlight in airlineTerminal.Flights)
            {
                double fees = entryFlight.Value.CalculateFees();
                double originalFees = entryFlight.Value.OriginalCalculateFees();
                double discountFees = Math.Abs(entryFlight.Value.DiscountCalculateFees());
                totalOriginalFees += originalFees;
                totalFlightDiscount += discountFees;
            }
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine($"{airlineTerminal.Name} total fee for the day.");
            Console.WriteLine("---------------------------------------------");
            double totalDiscount = airlineTerminal.DiscountCalculateFees() + totalFlightDiscount;
            double finalCost = totalOriginalFees - totalDiscount;
            Console.WriteLine($"Original cost: {totalOriginalFees:C2}");
            Console.WriteLine($"Total discount: {totalDiscount:C2}");
            Console.WriteLine($"Final cost: {totalOriginalFees:C2} - {totalDiscount:C2} = {finalCost:C2}");
        }
    }
}







