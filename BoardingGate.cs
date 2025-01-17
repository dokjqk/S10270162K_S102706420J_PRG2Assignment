using System;
using System.Collections.Generic;

public class BoardingGate
{
    // Properties for storing/indicationn
    public string GateName { get; private set; }
    public bool SupportsCFFT { get; private set; }
    public bool SupportsDDJB { get; private set; }
    public bool SupportsLWTT { get; private set; }
    public Flight Flight { get; set; }

    // Constructor to initialize the BoardingGate with the specified properties
    public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
    {
        GateName = gateName;
        SupportsCFFT = supportsCFFT;
        SupportsDDJB = supportsDDJB;
        SupportsLWTT = supportsLWTT;
    }

    // Method for fee calculation
    public double CalculateFees()
    {
        double fee = 0.0;
        if (SupportsCFFT) fee += 50.0;
        if (SupportsDDJB) fee += 40.0;
        if (SupportsLWTT) fee += 30.0;
        return fee;
    }


    public override string ToString()
    {
        return $"Gate: {GateName}, CFFT: {SupportsCFFT}, DDJB: {SupportsDDJB}, LWTT: {SupportsLWTT}, Flight: {Flight?.FlightNumber ?? "None"}";
    }
}
