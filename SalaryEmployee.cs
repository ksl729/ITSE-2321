using System;

namespace Lab6_Payroll_KLeach
{
    public class SalaryEmployee : Employee
    {
        public decimal WeeklySalary { get; set; }

        public SalaryEmployee(string firstName, string lastName, decimal weeklySalary)
            : base(firstName, lastName)
        {
            if (weeklySalary < 0)
                throw new ArgumentOutOfRangeException(nameof(weeklySalary), "Weekly salary must be >= 0");

            WeeklySalary = weeklySalary;
        }

        public override decimal Earnings()
        {
            return WeeklySalary;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Salaried Employee; Weekly Salary: {WeeklySalary:C}";
        }
    }
}
