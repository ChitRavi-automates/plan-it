using TeamApp.DTOs.TaskDTO;
using TeamApp.Models;
using TeamApp.Services;
using System.Reflection.Metadata.Ecma335;
using TeamApp.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace TeamApp.Endpoints.TaskEndpoints
{
    public class TaskEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            app.MapGet("/tasksall", async (TaskService taskservice) =>
            {
                var tasks = await taskservice.GetAllTasks();

                if (tasks == null) return Results.NotFound("Inga personer hittades");

                return Results.Ok(tasks);

            });

            app.MapGet("/users/{UserID}/tasks", async (int UserID, TaskService taskservice) =>
            {
                var tasks = await taskservice.GetTasksByUser(UserID);

                if (tasks == null)
                {
                    return Results.NotFound("Inga tasks hittade för denna användare");
                }

                return Results.Ok(tasks);
            });

            app.MapPost("/tasks", async (CreateTaskDTO taskDto, TaskService taskservice) =>
            {
                var newTask = await taskservice.AddTask(taskDto);
                Console.WriteLine(taskDto);
                Console.WriteLine("Endpoint anropad");
                if (newTask == null)
                {
                    return Results.BadRequest("Ingen task skapad");
                }

                return Results.Created($"/tasks/{newTask.TaskID}", newTask);
            });

            app.MapPatch("/tasks/{taskID}", async (int taskID, UpdateTaskDTO taskPatch, TaskService taskservice) =>
            {
                var updatedTask = await taskservice.UpdateTask(taskPatch, taskID);

                if (updatedTask == null)
                {
                    return Results.NotFound($"Task med ID {taskID} hittades inte.");
                }


                return Results.Ok(updatedTask);
            });

            app.MapGet("/tasks", async (string? search, string? status, int? assignedUser, string? orderBy, TaskService taskservice) =>
            {
                if (string.IsNullOrEmpty(orderBy)) { orderBy = "dueDate"; } //Default sortering

                var filteredTasks = await taskservice.FilterTasks(search, status, assignedUser, orderBy);

                if (!filteredTasks.Any())
                {
                    return Results.NotFound("Inga tasks matchade sökningen.");
                }

                foreach (var task in filteredTasks)
                {
                    
                }
                return Results.Ok(filteredTasks);
            });

            app.MapPut("/users/{userId}/teams/{teamId}", async (int userId, int teamId, TaskService taskservice) =>
            {
                var user = await taskservice.AddUserToTeam(userId, teamId);

                if (user == null)
                {
                    return Results.NotFound("Användare och/eller team hittades inte");
                }

                return Results.Ok(user);
            });

            app.MapGet("/teams/{teamId}/users", async (int teamId, TaskService taskservice) =>
            {
                var userlist = await taskservice.GetTeamMembers(teamId);

                if (userlist.IsNullOrEmpty())
                {
                    return Results.NotFound("Inga medlemmar hittades");
                }

                return Results.Ok(userlist);
            });

            app.MapGet("task/{taskId}", async (int taskId, TaskService taskservice) =>
            {
                var task = await taskservice.GetTaskById(taskId);

                if (task == null)
                {
                    return Results.NotFound("Ingen task hittades");
                }

                return Results.Ok(task);
            }
            );
        }
    }
}
