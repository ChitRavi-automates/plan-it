
using Microsoft.EntityFrameworkCore;
using TeamApp.Data;
using TeamApp.Endpoints.TeamEndpoints;
using TeamApp.Services;
using TeamApp.Endpoints.TaskEndpoints;
using TeamApp.Endpoints.CommenEndpoints;
using TeamApp.Endpoints.ProjectEndpoints;

namespace TeamApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<TeamDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<UserService>();

            builder.Services.AddScoped<TaskService>();
           
            builder.Services.AddScoped<CommentService>();

            builder.Services.AddScoped<ProjectService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyReactApp", policy =>
                {
                    // se till att l�nken h�r matchar din frontend!
                    policy.WithOrigins("http://localhost:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            builder.Services.AddHttpClient();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("MyReactApp");
            app.UseAuthorization();

            UserEndpoints.RegisterEndpoints(app);
            TaskEndpoints.RegisterEndpoints(app);
            CommentEndpoints.MessageEndpoints(app);
            ProjectEndpoints.RegisterEndpoints(app);

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<TeamDBContext>();
                context.Database.EnsureCreated(); // Or Migrate() if using migrations
                SeedData.InitialiseDB(context);
            }

            app.Run();
        }
    }
}
