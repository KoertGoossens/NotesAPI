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
/*                new Note
                {
                    Title = "title1",
                    Content = "content1",
                    CreatorId = 1
                },*/
            };

            await context.Notes.AddRangeAsync(Notes).ConfigureAwait(false);
            await context.SaveChangesAsync();
        }
    }
}
