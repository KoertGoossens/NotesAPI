using Data;
using Data.Models;

namespace DataSeeder.Seeders
{
	internal class NoteSeeder
	{
        public static async Task SeedNotes(DataContext context)
        {
            var Notes = new List<Note>
            {
            };

            await context.Notes.AddRangeAsync(Notes).ConfigureAwait(false);
            await context.SaveChangesAsync();
        }
    }
}
