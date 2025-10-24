using TeamApp.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TeamApp.DTOs.CommentDTO;
using TeamApp.DTOs.ProjectDTO;
using TeamApp.DTOs.TaskDTO;
using TeamApp.DTOs.TeamDTO;
using TeamApp.DTOs.UserDTO;
using TeamApp.Models;

namespace TeamApp.Services
{
    public class TaskService
    {
        private readonly TeamDBContext context;

        //Dependancy injection
        public TaskService(TeamDBContext _context)
        {
            context = _context;
        }

        public async Task<List<TaskDTO>> GetAllTasks()
        {
            var taskList = await context.Tasks.Select(t => new TaskDTO
            {
                TaskID = t.TaskID,
                Title = t.Title,
                DueDate = t.DueDate,
                Description = t.Description
            }).ToListAsync();
            return taskList;
        }
        public async Task<List<TaskDTO>> GetTasksByUser(int userId)
        {
            var taskList = await context.UserTasks.Where(ut => ut.userID_FK == userId).Select(ut => new TaskDTO
            {
                TaskID = ut.task.TaskID,
                Title = ut.task.Title,
                DueDate = ut.task.DueDate,
                Status = ut.task.Status

            }).ToListAsync();

            return taskList;
        }
        public async Task<TaskDTO> GetTaskById(int taskId)
        {
            var task = await context.Tasks.Where(t => t.TaskID == taskId).Select(t => new TaskDTO
            {
                TaskID = t.TaskID,
                Title = t.Title,
                DueDate = t.DueDate,
                Status = t.Status,
                Description = t.Description
            }).FirstOrDefaultAsync();

            return task;
        }
        public async Task<TaskDTO?> AddTask(CreateTaskDTO taskDto)
        {
            Models.Task newTask = new Models.Task
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                Status = taskDto.Status,
                DueDate = taskDto.DueDate,
                GoalID_FK = taskDto.GoalID_FK//ändra
            };

            context.Tasks.Add(newTask);
            var taskSaved = await context.SaveChangesAsync();
            if (taskSaved == 0) return null;
            var newUserTask = new UserTask
            {
                userID_FK = taskDto.UserID_FK, //Ändra
                taskID_FK = newTask.TaskID
            };

            context.UserTasks.Add(newUserTask);
            var userTaskSaved = await context.SaveChangesAsync();
            if (userTaskSaved == 0) return null;

            return new TaskDTO
            {
                TaskID = newTask.TaskID,
                Title = newTask.Title,
                Description = newTask.Description,
                Status = newTask.Status,
                DueDate = newTask.DueDate,
                GoalID_FK = newTask.GoalID_FK
            };
        }

        public async Task<TaskDTO?> UpdateTask(UpdateTaskDTO patch, int id)
        {
            Models.Task updatedTask = await context.Tasks.FindAsync(id);

            if (updatedTask == null) { return null; }

            //Kollar vilka element som uppdaterats och ändrar dom.
            if (patch.Title != null) { updatedTask.Title = patch.Title; }
            if (patch.Description != null) { updatedTask.Description = patch.Description; }
            if (patch.Status != null) { updatedTask.Status = patch.Status; }
            if (patch.DueDate != null) { updatedTask.DueDate = patch.DueDate.Value; }
            //if (patch.UserID_FK != null) { updatedTask.UserID_FK = patch.UserID_FK.Value; }
            if (patch.GoalID_FK != null) { updatedTask.GoalID_FK = patch.GoalID_FK.Value; }

            try
            {
                await context.SaveChangesAsync();
            }

            catch (Exception ex) { return null; }

            return new TaskDTO
            {
                TaskID = updatedTask.TaskID,
                Title = updatedTask.Title,
                Description = updatedTask.Description,
                Status = updatedTask.Status,
                DueDate = updatedTask.DueDate,
                //UserID_FK = updatedTask.UserID_FK,
                GoalID_FK = updatedTask.GoalID_FK
            };
        }

        public async Task<List<TaskDTO>> FilterTasks(string? search, string? status, int? assignedUser, string orderBy)
        {
            var query = context.Tasks.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.Title.Contains(search) || t.Description.Contains(search));
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(t => t.Status == status);
            }

            if (assignedUser != null)
            {
                query = query.Where(t => t.UserTasks.Any(ut => ut.userID_FK == assignedUser));
            }

            if (orderBy == "dueDate")
            {
                query = query.OrderBy(t => t.DueDate);
            }

            if (orderBy == "title")
            {
                query = query.OrderBy(t => t.Title);
            }

            var taskList = await query.ToListAsync();

            return taskList.Select(t => new TaskDTO
            {
                TaskID = t.TaskID,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                DueDate = t.DueDate,
                //TeamMember = context.UserTasks.Include(ut => ut.user).FirstOrDefault(ut => ut.taskID_FK == t.TaskID)?.user?.Username
                TeamMembers = context.UserTasks
                    .Where(ut => ut.taskID_FK == t.TaskID)
                    .Select(ut => ut.user.Username).ToList()
            }).ToList();
        }

        public async Task<UserDTO> AddUserToTeam(int teamId, int userId)
        {
            User user = await context.Users.SingleOrDefaultAsync(u => u.UserID == userId);

            if (user == null)
            {
                return null;
            }

            bool teamExists = await context.Teams.AnyAsync(t => t.TeamID == teamId);

            if (!teamExists)
            {
                return null;
            }

            user.TeamID_FK = teamId;

            await context.SaveChangesAsync();

            return new UserDTO
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }
        public async Task<List<UserDTO>?> GetTeamMembers(int teamId)
        {
            var userlist = await context.Users.Where(u => u.TeamID_FK == teamId).Select(u => new UserDTO
            {
                UserID = u.UserID,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role
            }).ToListAsync();

            return userlist;
        }
    }
}