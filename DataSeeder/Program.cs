using Data;
using DataSeeder.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataSeeder
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var dbConnectionString = "Server=(localdb)\\MSSQLLOCALDB;Database=NotesDB;Trusted_Connection=True;ConnectRetryCount=3";
            
            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(dbConnectionString);
            });

            var dataContext = serviceCollection.BuildServiceProvider().GetService<DataContext>();
            await dataContext.Database.EnsureCreatedAsync();

            await UserSeeder.SeedUsers(dataContext).ConfigureAwait(false);
            await NoteSeeder.SeedNotes(dataContext).ConfigureAwait(false);

            await dataContext.DisposeAsync();
        }
    }
}
