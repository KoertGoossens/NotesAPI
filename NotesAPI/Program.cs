using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Data;
using NotesAPI.Middleware;
using Data.Repositories.NoteRepository;
using Data.Repositories.UserRepository;
using Logic.Services.AuthService;
using Logic.Services.NoteService;
using Logic.Services.UserService;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Logic;

namespace NotesAPI
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();

			builder.Services.AddSwaggerGen(options =>
			{
				options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
				{
					Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
					In = ParameterLocation.Header,
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey
				});

				options.OperationFilter<SecurityRequirementsOperationFilter>();
			});

			builder.Services.AddHttpContextAccessor();
			builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<INoteService, NoteService>();

			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddScoped<INoteRepository, NoteRepository>();

			builder.Services.AddDbContext<DataContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
							.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
						ValidateIssuer = false,
						ValidateAudience = false
					};
				});

#if DEBUG
			builder.Services.AddCors(options => options.AddPolicy(name: "AngularOrigins",
				policy =>
				{
					policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
				}));
#endif

			builder.Services.AddTransient<ExceptionHandlingMiddleware>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

#if DEBUG
			app.UseCors("AngularOrigins");
#endif

			app.UseHttpsRedirection();

			app.UseMiddleware<ExceptionHandlingMiddleware>();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
