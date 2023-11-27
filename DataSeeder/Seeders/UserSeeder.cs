using Data.Models;
using Data;

namespace DataSeeder.Seeders
{
	internal class UserSeeder
	{
		public static async Task SeedUsers(DataContext context)
        {
            var Users = new List<User>
            {
                new User
                {
                    Username = "hidde_deboer",
                    Email = "hiddedeboer@outlook.com",
                    PasswordHash = CreatePasswordHash("hiddewachtwoord"),
                    Role = "User",
                    Notes = new List<Note>
                    {
                        new Note
                        {
                            Title = "Bericht van Hidde",
                            Content = "Hallo, ik ben Hidde en dit is mijn eerste bericht!"
                        },
                        new Note
                        {
                            Title = "Tweede bericht van Hidde",
                            Content = "Laat ik nog een tweede berichtje achterlaten."
                        }
                    }
                },
                new User
                {
                    Username = "milou_mulder",
                    Email = "miloumulder@hotmail.com",
                    PasswordHash = CreatePasswordHash("onveiligwachtwoord"),
                    Role = "User",
                    Notes = new List<Note>
                    {
                        new Note
                        {
                            Title = "Bericht van Milou",
                            Content = "Ik ben Milou en dit is mijn eerste bericht!"
                        },
                        new Note
                        {
                            Title = "Jarig!",
                            Content = "Morgen ben ik jarig! Daarom nodig ik Hidde en Jan morgen uit voor een feestje."
                        },
                        new Note
                        {
                            Title = "Jammer",
                            Content = "Jammer dat er niemand reageerde. Wat een saaie berichtendienst."
                        }
                    }
                },
                new User
                {
                    Username = "jan_vanzanten",
                    Email = "janvanzanten@outlook.com",
                    PasswordHash = CreatePasswordHash("welkom123"),
                    Role = "Admin",
                    Notes = new List<Note>
                    {
                        new Note
                        {
                            Title = "Hoi",
                            Content = "Hoi, ik ben Jan en ik kom even kijken wat deze app voorstelt."
                        }
                    }
                }
            };

            await context.Users.AddRangeAsync(Users).ConfigureAwait(false);
            await context.SaveChangesAsync();
        }

        private static string CreatePasswordHash(string password)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return passwordHash;
        }
	}
}
