using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using ExpenseManager.Repository.Entities;
using ExpenseManager.Repository.Repositories;
using ExpenseManager.Repository.Repositories.Contracts;
using ExpenseManager.Service.Services;
using ExpenseManager.Service.Services.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace ExpenseManager.Api
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static readonly IConfiguration Configuration = new ConfigurationBuilder()
                                                    .SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsettings.json")
                                                    .Build();

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            await ConfigureMongoDb();

            var mongoDbIdentityConfig = GetMongoDbIdentitySettings();
            builder.Services.ConfigureMongoDbIdentity<ApplicationUser, ApplicationRole, Guid>(mongoDbIdentityConfig)
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = true;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = Dns.GetHostName(),
                    ValidAudience = Dns.GetHostName(),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwtSettings:key"])),
                };
            });

            //DI
            builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

            builder.Services.AddTransient<IUserRepository, UserRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        /// <summary>
        /// This method is used to configure the MongoDb like setting unique key
        /// </summary>
        /// <returns></returns>
        private static async Task ConfigureMongoDb()
        {
            var client = new MongoClient(Configuration.GetConnectionString("databaseConnectionString"));
            var database = client.GetDatabase(Configuration.GetConnectionString("databaseName"));
            var indexOptions = new CreateIndexOptions
            {
                Unique = true,
            };

            var categories = database.GetCollection<BsonDocument>("Categories");
            var indexesCursor = await categories.Indexes.ListAsync();
            var indexes = await indexesCursor.ToListAsync();
            if (!indexes.Any())
            {
                var indexKeys = Builders<BsonDocument>.IndexKeys.Combine(
                Builders<BsonDocument>.IndexKeys.Ascending("CategoryName"),
                Builders<BsonDocument>.IndexKeys.Ascending("UserId"));
                var indexModel = new CreateIndexModel<BsonDocument>(indexKeys, indexOptions);
                var _ = await categories.Indexes.CreateOneAsync(indexModel);
            }

            //var expenses = database.GetCollection<BsonDocument>("Expenses");
            //indexesCursor = await expenses.Indexes.ListAsync();
            //indexes = await indexesCursor.ToListAsync();
            //if (!indexes.Any())
            //{
            //    var indexKeys = Builders<BsonDocument>.IndexKeys.Combine(
            //    Builders<BsonDocument>.IndexKeys.Ascending("DateTimeStamp"),
            //    Builders<BsonDocument>.IndexKeys.Ascending("Statement"),
            //    Builders<BsonDocument>.IndexKeys.Ascending("SubExpenseName"),
            //    Builders<BsonDocument>.IndexKeys.Ascending("UserId"));
            //    var indexModels = new List<CreateIndexModel<BsonDocument>>
            //    {
            //        new CreateIndexModel<BsonDocument>(indexKeys, indexOptions)
            //    };
            //    var _ = await categories.Indexes.CreateOneAsync(indexModel);
            //}
        }

        /// <summary>
        /// It is used to get the settings value for MongoDbIdentity
        /// </summary>
        /// <returns></returns>
        private static MongoDbIdentityConfiguration GetMongoDbIdentitySettings()
        {
            return new MongoDbIdentityConfiguration
            {
                MongoDbSettings = new MongoDbSettings
                {
                    ConnectionString = Configuration.GetConnectionString("databaseConnectionString"),
                    DatabaseName = Configuration.GetConnectionString("databaseName")
                },
                IdentityOptionsAction = options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireDigit = true;

                    options.User.RequireUniqueEmail = true;
                }
            };
        }
    }
}