using System;
using MortgageCalculatorLibrary; 
using Spectre.Console;

namespace MortgageCalcUI
{
   public class Program
    {
        static void Main(string[] args)
        {
            // Prompt user for input using Spectre.Console
            double loanAmount = AnsiConsole.Prompt(
                new TextPrompt<double>("Enter the [green]loan amount[/]:")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]That's not a valid number![/]")
                    .Validate(value => value > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]Loan amount must be greater than 0![/]"))
            );

            double downPayment = AnsiConsole.Prompt(
                new TextPrompt<double>("Enter the [green]down payment[/]:")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]That's not a valid number![/]")
                    .Validate(value => value >= 0 ? ValidationResult.Success() : ValidationResult.Error("[red]Down payment cannot be negative![/]"))
            );

            double annualInterestRate = AnsiConsole.Prompt(
                new TextPrompt<double>("Enter the [green]annual interest rate (in %)[/]:")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]That's not a valid number![/]")
                    .Validate(value => value > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]Interest rate must be greater than 0![/]"))
            );

            int loanTermYears = AnsiConsole.Prompt(
                new TextPrompt<int>("Enter the [green]loan term (in years)[/]:")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]That's not a valid number![/]")
                    .Validate(value => value > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]Loan term must be greater than 0![/]"))
            );

            // Simulate the progress of calculating the mortgage with a progress bar
            AnsiConsole.Progress()
                .Start(ctx =>
                {
                    // Define a task for the progress bar
                    var task = ctx.AddTask("[green]Calculating mortgage...[/]");

                    // Simulate some work by incrementing the progress bar
                    while (!ctx.IsFinished)
                    {
                        // Simulate processing by waiting
                        Thread.Sleep(100); // Simulate work with a delay

                        // Increment the progress bar
                        task.Increment(1.5);
                    }
                });

            // Create an instance of the MortgageCalculator
            var calculator = new MortgageCalculator(loanAmount, downPayment, annualInterestRate, loanTermYears);

            // Calculate the monthly payment
            double monthlyPayment = calculator.CalculateMonthlyPayment();

            // Display the result
            AnsiConsole.MarkupLine($"Your [yellow]monthly payment[/] is: [bold green]{monthlyPayment:C2}[/]");

            // Ask if the user wants to see the amortization schedule
            if (AnsiConsole.Confirm("Do you want to see the amortization schedule?"))
            {
                AnsiConsole.WriteLine();
                DisplayAmortizationSchedule(calculator, loanAmount, downPayment, annualInterestRate, loanTermYears, monthlyPayment);
            }
        }

        static void DisplayAmortizationSchedule(MortgageCalculator calculator, double loanAmount, double downPayment, double annualInterestRate, int loanTermYears, double monthlyPayment)
        {
            // Calculate values for the amortization schedule
            double principal = loanAmount - downPayment;
            double monthlyInterestRate = annualInterestRate / 100 / 12;
            int numberOfPayments = loanTermYears * 12;
            double remainingBalance = principal;

            // Create a table with color-coordinated columns
            var table = new Table();
            table.AddColumn("[bold yellow]Month[/]");
            table.AddColumn("[bold red]Interest Payment[/]");
            table.AddColumn("[bold green]Principal Payment[/]");
            table.AddColumn("[bold blue]Remaining Balance[/]");

            // Add rows to the table with color-coded values
            for (int month = 1; month <= numberOfPayments; month++)
            {
                double interestPayment = remainingBalance * monthlyInterestRate;
                double principalPayment = monthlyPayment - interestPayment;
                remainingBalance -= principalPayment;

                table.AddRow(
                    $"[yellow]{month}[/]",
                    $"[red]{interestPayment:C2}[/]",
                    $"[green]{principalPayment:C2}[/]",
                    $"[blue]{remainingBalance:C2}[/]"
                );
            }

            AnsiConsole.Write(table);
        }
    }
}