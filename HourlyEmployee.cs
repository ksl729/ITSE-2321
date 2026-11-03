using System;

namespace Employee_Payroll_Form_KLeach
{
    public class HourlyEmployee : Employee
    {
        public decimal HourlyWage { get; set; }
        public decimal HoursWorked { get; set; }

        public decimal Wage
        {
            get => HourlyWage;
            set => HourlyWage = value;
        }

        public decimal Hours
        {
            get => HoursWorked;
            set => HoursWorked = value;
        }

        public HourlyEmployee(string firstName, string lastName, decimal hourlyWage, decimal hoursWorked)
            : base(firstName, lastName)
        {
            if (hourlyWage < 0)
                throw new ArgumentOutOfRangeException(nameof(hourlyWage), "Hourly wage must be >= 0");
            if (hoursWorked < 0)
                throw new ArgumentOutOfRangeException(nameof(hoursWorked), "Hours worked must be >= 0");

            HourlyWage = hourlyWage;
            HoursWorked = hoursWorked;
        }

        public override decimal Earnings()
        {
            return HourlyWage * HoursWorked;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Hourly Employee; Hourly Wage: {HourlyWage:C}, Hours Worked: {HoursWorked}";
        }
    }
}
