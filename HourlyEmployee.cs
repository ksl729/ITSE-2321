using System;

namespace Lab6_Payroll_KLeach
{
    public class HourlyEmployee : Employee
    {
        public decimal Wage { get; set; } // hourly wage
        public decimal Hours { get; set; } // hours worked in a week

        public HourlyEmployee(string firstName, string lastName, decimal wage, decimal hours)
            : base(firstName, lastName)
        {
            if (wage < 0)
                throw new ArgumentOutOfRangeException(nameof(wage), "Wage must be >= 0");
            if (hours < 0 || hours > 168)
                throw new ArgumentOutOfRangeException(nameof(hours), "Hours must be between 0 and 168");

            Wage = wage;
            Hours = hours;
        }

        public override decimal Earnings()
        {
            const decimal overtimeRate = 1.5m;
            const decimal standardHours = 40m;

            if (Hours <= standardHours)
            {
                return Wage * Hours;
            }

            var overtimeHours = Hours - standardHours;
            return (Wage * standardHours) + (Wage * overtimeHours * overtimeRate);
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Hourly Employee; Hours: {Hours:F2}, Hourly Wage: {Wage:C}";
        }
    }
}
