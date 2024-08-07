using MortgageCalculatorLibrary;

namespace MortgageCalculatorTests
{
    [TestClass]
    public class MortgageCalculatorTests
    {
        [TestMethod]
        public void CalculateMonthlyPayment_Test()
        {
            double loanAmount = 300000;
            double downPayment = 60000;
            double annualInterestRate = 5;
            int loanTermYears = 30;
            MortgageCalculator calculator = new MortgageCalculator(loanAmount, downPayment, annualInterestRate, loanTermYears);
            double monthlyPayment = calculator.CalculateMonthlyPayment();
            double expectedMonthlyPayment = 1288.37;
            Assert.AreEqual(expectedMonthlyPayment, monthlyPayment, 0.01);
        }

        [TestMethod]
        public void CalculateMonthlyPayment_ZeroDownPayment_Test()
        {
            double loanAmount = 300000;
            double downPayment = 0;
            double annualInterestRate = 5;
            int loanTermYears = 30;
            MortgageCalculator calculator = new MortgageCalculator(loanAmount, downPayment, annualInterestRate, loanTermYears);
            double monthlyPayment = calculator.CalculateMonthlyPayment();
            double expectedMonthlyPayment = 1610.46;
            Assert.AreEqual(expectedMonthlyPayment, monthlyPayment, 0.01);
        }

        [TestMethod]
        public void CalculateAmortizationSchedule_Test()
        {
            double loanAmount = 300000;
            double downPayment = 60000;
            double annualInterestRate = 5;
            int loanTermYears = 30;
            MortgageCalculator calculator = new MortgageCalculator(loanAmount, downPayment, annualInterestRate, loanTermYears);

            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);
                calculator.CalculateAmortizationSchedule();
                string output = sw.ToString();
                Assert.IsTrue(output.Contains("Month 1"));
                Assert.IsTrue(output.Contains("Remaining Balance"));
            }
        }
    }
}