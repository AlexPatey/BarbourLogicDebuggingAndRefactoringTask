using DebuggingAndRefactoringTask1;
using DebuggingAndRefactoringTask1.Models;
using DebuggingAndRefactoringTask1.Services;
using DebuggingAndRefactoringTask1.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BankingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Setup DI container/provider - I am using DI here as in a real scenario as the project develops you would want a Data layer etc., 
            and DI makes mocking those dependencies in your unit tests a lot easier, etc.*/
            var services = new ServiceCollection();

            services.AddSingleton<IBankingService, BankingService>();
            services.AddSingleton<Application>();

            var serviceProvider = services.BuildServiceProvider();

            var application = serviceProvider.GetRequiredService<Application>();

            //Run application
            application.Run();
        }
    }
}
