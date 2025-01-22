using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10270162K_S102706420J_PRG2Assignment
{
    class CFFTFlight : Flight
    {
        private double requestFee;
        public double RequestFee
        {
            get { return requestFee; }
            set { requestFee = value; }
        }

        public CFFTFlight() { }

        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee) : base(flightNumber, origin, destination, expectedTime, status)
        {
            RequestFee = requestFee;
        }

        public override double CalculateFees()
        {
            double fees = 0;
            fees += 300;
            if (origin == "Singapore (SIN)")
            {
                fees += 800;
            }
            if (destination == "Singapore (SIN)")
            {
                fees += 500;
            }
            if (ExpectedTime.TimeOfDay < TimeSpan.FromHours(11) || ExpectedTime.TimeOfDay > TimeSpan.FromHours(21))
            {
                fees -= 110;
            }
            if (origin == "Dubai (DXB)" || origin == "Bangkok (BKK)" || origin == "Tokyo (NRT)")
            {
                fees -= 25;
            }
            fees += 150;
            return fees;
        }

        public override string ToString()
        {
            return base.ToString() + $" requestFee {requestFee}";
        }

    }
}
