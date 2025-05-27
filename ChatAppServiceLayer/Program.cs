
using ChatAppDataAccessLayer;
using Microsoft.Extensions.DependencyInjection;

namespace ChatAppServiceLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.FullName); // Use fully qualified names
            });


            builder.Services.AddTransient<ChatAppDbContext>();
            builder.Services.AddTransient<ChatAppRepository>(
                c => new ChatAppRepository(c.GetRequiredService<ChatAppDbContext>()));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials(); // Only if you're using cookies
                    });
            });
            builder.Services.AddSignalR();

          



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapHub<ChatHub>("/chathub");
            app.MapControllers();
            // Fix the middleware order - put CORS after HTTPS redirection
            app.UseHttpsRedirection();
            // Replace your existing CORS setup with this
            app.UseCors("AllowFrontend");

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
