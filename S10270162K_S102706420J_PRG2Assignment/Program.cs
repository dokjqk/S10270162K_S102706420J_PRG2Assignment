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
Console.WriteLine(terminal5.ToString());

// 2. Load files (flights) (Ahmad)
// 3. List all flights with their basic information (Ahmad)
// 4. List all boarding gates (Hendi)
// 5. Assign a boarding gate to a flight (Ahmad)
// 6. Create a new flight (Ahmad)
// 7. Display full flight details from an airline (Hendi)
// 8. Modify flight details (Hendi)
// 9. Display scheduled flights in chronological order, with boarding gates assignments where applicable (Ahmad)
// Advanced Features

