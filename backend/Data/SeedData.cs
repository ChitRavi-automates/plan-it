using TeamApp.Models;

namespace TeamApp.Data
{
    public class SeedData
    {
        public static void InitialiseDB(TeamDBContext context)
        {
            if (context.Users.Any() || context.Projects.Any() || context.Tasks.Any()) { return; }


            var teams = new List<Team> {
                new() {
                Name = "Team 1",
                Description = "Team number one"
                },
                                new() {
                Name = "Team 2",
                Description = "Team number two"
                },
                                                new() {
                Name = "Team 3",
                Description = "Team number three"
                },
            };
            context.Teams.AddRange(teams);

            context.SaveChanges();
            var seedUsers = new List<User> {
                new() {
                    Username = "Alice",
                    PasswordHash = "hash1",
                    Team = teams[0],
                    Email = "alice@example.com",
                    Role = "Developer",
                    Description = "Alice is a frontend developer working on project Alpha."
                },
                new() {
                    Username = "Bob",
                    PasswordHash = "hash2",
                    Team = teams[1],
                    Email = "bob@example.com",
                    Role = "Developer",
                    Description = "Bob is a backend developer working on project Beta."
                },
                new() {
                    Username = "Eric",
                    PasswordHash = "hash3",
                    Team = teams[1],
                    Email = "eric@example.com",
                    Role = "QA",
                    Description = "Eric works with quality assurance"
                },
                new() {
                    Username = "Carol",
                    PasswordHash = "hash4",
                    Team = teams[0],
                    Email = "carol@example.com",
                    Role = "Designer",
                    Description = "Carol is a UX/UI designer for project Alpha."
                },
                new() {
                    Username = "Dave",
                    PasswordHash = "hash5",
                    Team = teams[1],
                    Email = "dave@example.com",
                    Role = "DevOps",
                    Description = "Dave manages deployment and infrastructure."
                },
                new() {
                    Username = "Fiona",
                    PasswordHash = "hash6",
                    Team = teams[2],
                    Email = "fiona@example.com",
                    Role = "Product Manager",
                    Description = "Fiona oversees product development and strategy."
                },
                new() {
                    Username = "George",
                    PasswordHash = "hash7",
                    Team = teams[2],
                    Email = "george@example.com",
                    Role = "QA",
                    Description = "George is a quality assurance engineer focusing on automation."
                }
            };
            context.Users.AddRange(seedUsers);
            context.SaveChanges();
            var seedGoals = new List<Project> {
                new(){
                    Title = "Project Alpha",
                    Description = "Description for Project Alpha",
                    Deadline = DateTime.Now.AddDays(10),
                    User = seedUsers[0]
                },
                new(){
                    Title = "Project Beta",
                    Description = "Description for Project Beta",
                    Deadline = DateTime.Now.AddDays(20),
                    User = seedUsers[1]
                }
            };
            context.Projects.AddRange(seedGoals);
            context.SaveChanges();

            var seedTasks = new List<Models.Task>
            {
                new()
                {
                    Goal = seedGoals[0],
                    Title = "Design UI",
                    Status = "Ej påbörjad",
                    Description = "Create mockups",
                    DueDate = new DateTime(2025, 5, 15)
                },
                new()
                {
                    Goal = seedGoals[0],
                    Title = "Design UI",
                    Status = "Ej påbörjad",
                    Description = "Create mockups",
                    DueDate = new DateTime(2025, 5, 15)
                },
                new()
                {
                    Goal = seedGoals[0],
                    Title = "Design UI",
                    Status = "Ej påbörjad",
                    Description = "Create mockups",
                    DueDate = new DateTime(2025, 5, 15)
                },
                new()
                {
                    Goal = seedGoals[1],
                    Title = "API Integration",
                    Status = "Påbörjad",
                    Description = "Connect backend",
                    DueDate = new DateTime(2025, 5, 14)
                },
                new()
                {
                    Goal = seedGoals[1],
                    Title = "Build Frontend",
                    Status = "Påbörjad",
                    Description = "React Project",
                    DueDate = new DateTime(2025, 5, 23)
                },
                new()
                {
                    Goal = seedGoals[1],
                    Title = "API Integration",
                    Status = "Påbörjad",
                    Description = "Connect backend",
                    DueDate = new DateTime(2025, 5, 14)
                },
                new()
                {
                    Goal = seedGoals[0],
                    Title = "Write Tests",
                    Status = "Avslutad",
                    Description = "Unit tests",
                    DueDate = new DateTime(2025, 5, 10)
                }
            };

            context.AddRange(seedTasks);
            context.SaveChanges();

            var seedUserTasks = new List<UserTask> {
                new() { user = seedUsers[0], task = seedTasks[0], userID_FK = seedUsers[0].UserID, taskID_FK = seedTasks[0].TaskID},
                new() { user = seedUsers[0], task = seedTasks[1], userID_FK = seedUsers[0].UserID, taskID_FK = seedTasks[1].TaskID },
                new() { user = seedUsers[0], task = seedTasks[2], userID_FK = seedUsers[0].UserID, taskID_FK = seedTasks[2].TaskID },
                new() { user = seedUsers[0], task = seedTasks[6], userID_FK = seedUsers[0].UserID, taskID_FK = seedTasks[6].TaskID },

                new() { user = seedUsers[1], task = seedTasks[3], userID_FK = seedUsers[1].UserID, taskID_FK = seedTasks[3].TaskID },
                new() { user = seedUsers[1], task = seedTasks[4], userID_FK = seedUsers[1].UserID, taskID_FK = seedTasks[4].TaskID },
                new() { user = seedUsers[1], task = seedTasks[5], userID_FK = seedUsers[1].UserID, taskID_FK = seedTasks[5].TaskID }
            };
            context.UserTasks.AddRange(seedUserTasks);

            context.SaveChanges();
        }
    }
}
