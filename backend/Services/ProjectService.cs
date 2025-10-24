using TeamApp.Data;
using TeamApp.DTOs.ProjectDTO;
using TeamApp.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace TeamApp.Services
{
    public class ProjectService
    {
        private readonly TeamDBContext context;

        public ProjectService(TeamDBContext context)
        {
            this.context = context;
        }

        public async Task<List<ProjectDTO>> GetAllProjectsAsync()
        {
            return await context.Projects.Select(g => new ProjectDTO
            {
                ProjectID = g.ProjectID,
                Title = g.Title,
                Description = g.Description,
                Deadline = g.Deadline,
            }).ToListAsync();
        }

        public async Task<List<ProjectDTO>> GetProjectsByUserAsync(int userId)
        {
            return await context.Projects
                .Where(g => g.UserID_FK == userId)
                .Select(g => new ProjectDTO
                {
                    ProjectID = g.ProjectID,
                    Title = g.Title,
                    Description = g.Description,
                    Deadline = g.Deadline,
                }).ToListAsync();
        }

        public async Task<ProjectDTO?> GetProjectByIdAsync(int id)
        {
            var Project = await context.Projects.FindAsync(id);
            if (Project == null) return null;

            return new ProjectDTO
            {
                ProjectID = Project.ProjectID,
                Title = Project.Title,
                Description = Project.Description,
                Deadline = Project.Deadline,
            };
        }

        public async Task<ProjectDTO> CreateProjectAsync(CreateProjectDTO dto)
        {
            var Project = new Project
            {
                UserID_FK = dto.UserID,
                Title = dto.Title,
                Description = dto.Description,
                Deadline = DateTime.Now.AddDays(30),
            };

            context.Projects.Add(Project);
            await context.SaveChangesAsync();

            return new ProjectDTO
            {
                ProjectID = Project.ProjectID,
                Title = Project.Title,
                Description = Project.Description,
                Deadline = Project.Deadline,
            };
        }

        public async Task<bool> UpdateProjectAsync(UpdateProjectDTO dto)
        {
            var Project = await context.Projects.FindAsync(dto.ProjectID);
            if (Project == null) return false;

            Project.UserID_FK = dto.UserID;
            Project.Title = dto.Title;
            Project.Description = dto.Description;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var Project = await context.Projects.FindAsync(id);
            if (Project == null) return false;

            context.Projects.Remove(Project);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
