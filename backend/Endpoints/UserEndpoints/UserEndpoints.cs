using System.Reflection.Metadata.Ecma335;
using TeamApp.Services;
using TeamApp.DTOs;
using TeamApp.DTOs.TaskDTO;
using Microsoft.IdentityModel.Tokens;
using TeamApp.DTOs.UserDTO;

namespace TeamApp.Endpoints.TeamEndpoints
{
    public class UserEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            //Hämtar lista på alla users
            app.MapGet("/Users", async (UserService userservice) =>
            {
                var user =  await userservice.GetAllUsers();

                if(user == null) return Results.NotFound("Inga personer hittades");

                return Results.Ok(user);

            });

            //Skapar en user
            app.MapPost("/users", async (UserService userService, CreateUserDTO newUser) =>
            {
                var createdUser = await userService.CreateUser(newUser);

                if (createdUser == null) { return Results.BadRequest("Skapandet av en ny användare misslyckades"); }

                return Results.Ok(createdUser);
            });

            //Uppdaterar en user
            app.MapPut("/users/{id}", async (UserService userService, UpdateUserDTO userUpdate, int id) =>
            {
                if (userUpdate.UserID != id)
                {
                    return Results.BadRequest("Finns ingen användare med det ID:t!");
                }

                var result = await userService.UpdateUser(userUpdate);

                if (result == null) { return Results.NotFound("Användaren hittades inte"); }

                return Results.Ok(result);
            });

            //Tar bort en user
            app.MapDelete("/users/{id}", async (UserService userService, int id) =>
            {
                var result = await userService.DeleteUser(id);

                if (!result) { return Results.NotFound("Användaren hittades inte"); }

                return Results.Ok(result);
            });

            app.MapPost("/users/login", async (UserService userService, LoginRequest request) =>
            {
                if(string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password)) 
                {
                    return Results.BadRequest("E-post och lösenord krävs");
                }

                var user = await userService.Authenticate(request.Email, request.Password);

                if(user == null) { return Results.Unauthorized(); }

                return Results.Ok(new {user.UserID, user.Role });
            });

        }
    }
}
