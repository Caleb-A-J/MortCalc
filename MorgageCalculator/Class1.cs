using System;

namespace MortgageCalculatorLibrary
{
    public class MortgageCalculator
    {
        public double LoanAmount { get; private set; }
        public double DownPayment { get; private set; }
        public double AnnualInterestRate { get; private set; }
        public int LoanTermYears { get; private set; }

        public MortgageCalculator(double loanAmount, double downPayment, double annualInterestRate, int loanTermYears)
        {
            LoanAmount = loanAmount;
            DownPayment = downPayment;
            AnnualInterestRate = annualInterestRate;
            LoanTermYears = loanTermYears;
        }

        public double CalculateMonthlyPayment()
        {
            double principal = LoanAmount - DownPayment;
            double monthlyInterestRate = AnnualInterestRate / 100 / 12;
            int numberOfPayments = LoanTermYears * 12;

            double monthlyPayment = (principal * monthlyInterestRate * Math.Pow(1 + monthlyInterestRate, numberOfPayments)) /
                                    (Math.Pow(1 + monthlyInterestRate, numberOfPayments) - 1);
            return monthlyPayment;
        }

        public void CalculateAmortizationSchedule()
        {
            double principal = LoanAmount - DownPayment;
            double monthlyInterestRate = AnnualInterestRate / 100 / 12;
            int numberOfPayments = LoanTermYears * 12;
            double monthlyPayment = CalculateMonthlyPayment();
            double remainingBalance = principal;

            for (int month = 1; month <= numberOfPayments; month++)
            {
                double interestPayment = remainingBalance * monthlyInterestRate;
                double principalPayment = monthlyPayment - interestPayment;
                remainingBalance -= principalPayment;

                Console.WriteLine($"Month {month}:");
                Console.WriteLine($"Interest Payment: {interestPayment:F2}");
                Console.WriteLine($"Principal Payment: {principalPayment:F2}");
                Console.WriteLine($"Remaining Balance: {remainingBalance:F2}");
                Console.WriteLine();
            }
        }
    }
}
