using System;

namespace Lab6_Payroll_KLeach
{
    public class CommissionEmployee : Employee
    {
        // Gross sales for the week
        public decimal GrossSales { get; set; }

        // Commission rate as a decimal (e.g., 0.25 for 25%)
        public decimal CommissionRate { get; set; }

        public CommissionEmployee(string firstName, string lastName, decimal grossSales, decimal commissionRate)
            : base(firstName, lastName)
        {
            if (grossSales < 0)
                throw new ArgumentOutOfRangeException(nameof(grossSales), "Gross sales must be >= 0");
            if (commissionRate < 0 || commissionRate > 1)
                throw new ArgumentOutOfRangeException(nameof(commissionRate), "Commission rate must be between 0 and 1");

            GrossSales = grossSales;
            CommissionRate = commissionRate;
        }

        public override decimal Earnings()
        {
            return GrossSales * CommissionRate;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Commission Employee; Weekly Sales: {GrossSales:C}, Commission Rate: {CommissionRate:P}";
        }
    }
}
