using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TeamApp.Data;
using TeamApp.DTOs.CommentDTO;
using TeamApp.DTOs.ProjectDTO;
using TeamApp.DTOs.TaskDTO;
using TeamApp.DTOs.TeamDTO;
using TeamApp.DTOs.UserDTO;
using TeamApp.Models;

namespace TeamApp.Services
{
    public class UserService
    {
        private readonly TeamDBContext context;
        private readonly ILogger<UserService> logger;


        //Dependancy injection
        public UserService(TeamDBContext _context, ILogger<UserService> _logger)
        {
            context = _context;
            logger = _logger;
        }

        public async Task<List<ProjectDTO>> GetAllProjects()
        {
            var projectList = await context.Projects.Select(g => new ProjectDTO
            {
                ProjectID = g.ProjectID,
                Title = g.Title,
                Description = g.Description,
                Deadline = g.Deadline
            }).ToListAsync();
            return projectList;
        }

        public async Task<List<TaskDTO>> GetAllTasks()
        {
            var taskList = await context.Tasks.Select(t => new TaskDTO
            {
                TaskID = t.TaskID,
                Title = t.Title,
                DueDate = t.DueDate,
            }).ToListAsync();
            return taskList;
        }

        public async Task<List<TeamDTO>> GetAllTeams()
        {
            var teamList = await context.Teams.Select(t => new TeamDTO
            {
                TeamID = t.TeamID,
                Description = t.Description,
                Name = t.Name
            }).ToListAsync();
            return teamList;
        }


        /*----------------------------------------------------------------------------------------------- */
        /*---------------------------------*******USER******-------------------------------------------------------------- */

        public async Task<List<UserDTO>> GetAllUsers()
        {
            var userList = await context.Users.Select(u => new UserDTO
            {
                UserID = u.UserID,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,
                Description = u.Description
            }).ToListAsync();
            return userList;
        }

        public async Task<UserDTO?> GetUserByID(int userID)
        {
            var user = await context.Users.Where(u => u.UserID == userID).Select(u => new UserDTO
            {
                Username = u.Username,
                Email = u.Email,
                Role = u.Role
            }).FirstOrDefaultAsync();
            return user;
        }

        public async Task<UserDTO?> CreateUser(CreateUserDTO newUser)
        {
            var validationContext = new ValidationContext(newUser);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(newUser, validationContext, validationResults, true);

            if (!isValid)
            {
                throw new ValidationException("Valideringen misslyckades för den nya användaren");
            }

            var user = new User
            {
                Email = newUser.Email,
                Username = newUser.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.Password), // Hashing här
                Role = newUser.Role ?? "User",
                Description = newUser.Description
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new UserDTO
            {
                UserID = user.UserID,
                Email = user.Email,
                Role = user.Role,
                Username = user.Username,
                Description = user.Description
            };
        }

        public async Task<UserDTO?> UpdateUser(UpdateUserDTO userUpdate)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserID == userUpdate.UserID);
            if (user == null) { return null; }

            user.Username = userUpdate.Username;
            user.Email = userUpdate.Email;

            await context.SaveChangesAsync();

            return new UserDTO
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserID == id);
            if (user == null) { return false; }

            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return true;

        }
        
        //Metod för säker inloggning
        public async Task<User?> Authenticate(string email, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                logger.LogWarning($"Inloggningsförsök med okänd e-post: {email}");
                return null;
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                logger.LogWarning($"Felaktigt lösenord för användare: {user.Email}");
                return null;
            }

            return user;
        }

        /*----------------------------------------------------------------------------------------------- */
        /*----------------------------------------------------------------------------------------------- */


    }
}
