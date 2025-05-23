using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskManagerService.Data;
using TaskManagerService.Core.Models;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var optionsBuilder = new DbContextOptionsBuilder<TaskManagerDbContext>();
optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

using var context = new TaskManagerDbContext(optionsBuilder.Options);

try
{
    Console.WriteLine("Applying migrations...");
    context.Database.Migrate();
    Console.WriteLine("Migrations applied successfully!");
    return 0;
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
    return 1;
}
