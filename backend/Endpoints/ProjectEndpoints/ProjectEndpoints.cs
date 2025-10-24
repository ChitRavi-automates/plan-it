using TeamApp.Services;
using TeamApp.DTOs.ProjectDTO;

namespace TeamApp.Endpoints.ProjectEndpoints
{
    public class ProjectEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            var projectGroup = app.MapGroup("/api/projects").WithTags("Projects");

            // GET: api/projects --> Alla projekt
            projectGroup.MapGet("/", async (ProjectService projectService) =>
            {
                var projects = await projectService.GetAllProjectsAsync();

                if (projects == null || !projects.Any())
                {
                    return Results.NotFound("Inga projekt hittades");
                }
                else
                {
                    return Results.Ok(projects);
                }
            }).WithName("GetAllProjects");

            // GET: api/project/user/{userId} --> Projekt för en viss användare
            projectGroup.MapGet("/user/{userId}", async (int userId, ProjectService projectService) =>
            {
                var projects = await projectService.GetProjectsByUserAsync(userId);

                if (projects == null || !projects.Any())
                {
                    return Results.NotFound($"Inga projekt hittades för användare med ID {userId}");
                }
                else
                {
                    return Results.Ok(projects);
                }
            }).WithName("GetProjectsByUser");


            // GET: api/projects/{id} --> projekt med ett specifikt id
            projectGroup.MapGet("/{id}", async (int id, ProjectService projectService) =>
            {
                var project = await projectService.GetProjectByIdAsync(id);

                if (project == null)
                {
                    return Results.NotFound($"Projekt med ID {id} hittades inte");
                }
                else
                {
                    return Results.Ok(project);
                }
            }).WithName("GetProjectById");


            // POST: api/projects --> Skapa ett nytt projekt
            projectGroup.MapPost("/", async (CreateProjectDTO createprojectDto, ProjectService projectService) =>
            {
                if (createprojectDto == null)
                {
                    return Results.BadRequest("Ogiltig data för projekt");
                }

                var createdproject = await projectService.CreateProjectAsync(createprojectDto);

                if (createdproject == null)
                {
                    return Results.Problem("Något gick fel vid skapandet av projektet");
                }

                return Results.CreatedAtRoute("GetprojectById", new { id = createdproject.ProjectID }, createdproject);
            }).WithName("CreateProject");



            // PUT: api/projects/{id} --> Uppdatera ett projekt
            projectGroup.MapPut("/{id}", async (int id, UpdateProjectDTO updateprojectDto, ProjectService projectService) =>
            {
                if (id != updateprojectDto.ProjectID)
                {
                    return Results.BadRequest("ID-matchning misslyckades");
                }

                var result = await projectService.UpdateProjectAsync(updateprojectDto);

                if (result)
                {
                    return Results.NoContent();
                }
                else
                {
                    return Results.NotFound($"Projekt med ID {id} hittades inte");
                }
            }).WithName("UpdateProject");


            // DELETE: api/projects/{id} --> Radera ett projekt
            projectGroup.MapDelete("/{id}", async (int id, ProjectService projectService) =>
            {
                var result = await projectService.DeleteProjectAsync(id);
                if (result)
                {
                    return Results.NoContent();
                }
                else
                {
                    return Results.NotFound($"Projekt med ID {id} hittades inte");
                }
            }).WithName("DeleteProject");
        }
    }
}