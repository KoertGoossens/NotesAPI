using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
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
using Serilog;
using FluentValidation;
using System;
using Logic.Dtos.User;
using NotesAPI.Validation.User;
using Logic.Dtos.Note;
using NotesAPI.Validation.Note;

namespace NotesAPI
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

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

			builder.Services.AddScoped<IValidator<LoginUserDto>, LoginUserDtoValidator>();
			builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>();
			builder.Services.AddScoped<IValidator<CreateNoteDto>, CreateNoteDtoValidator>();
			builder.Services.AddScoped<IValidator<UpdateNoteDto>, UpdateNoteDtoValidator>();

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

			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console()
				.WriteTo.File("Logs/NotesAPILog-.txt", rollingInterval: RollingInterval.Day)
				.CreateLogger();

			var app = builder.Build();


			// HTTP request pipeline

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

#if DEBUG
			app.UseCors("AngularOrigins");
#endif

			app.UseHttpsRedirection();

			

			app.UseAuthorization();

			app.MapControllers();

			app.UseMiddleware<ExceptionHandlingMiddleware>();

			app.Run();
		}
	}
}
