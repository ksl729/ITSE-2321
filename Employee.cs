using System;

namespace Lab6_Payroll_KLeach
{
    // Abstract base class Employee
    public abstract class Employee
    {
        public string FirstName { get; }
        public string LastName { get; }

        protected Employee(string firstName, string lastName)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        }

        // Must be implemented by derived classes to calculate weekly earnings
        public abstract decimal Earnings();

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
