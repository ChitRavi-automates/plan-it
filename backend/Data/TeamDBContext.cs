using Microsoft.EntityFrameworkCore;
using TeamApp.Models;
using Task = TeamApp.Models.Task;

namespace TeamApp.Data
{
    public class TeamDBContext : DbContext
    {
        public TeamDBContext(DbContextOptions<TeamDBContext> options) : base(options)
        {
            
        }

        /*Behövde lägga till denna metod eftersom UserTask har två FK, vilket
        inte är tillåtet. Metoden tillåter att klassen kan ha två FK, och inte påverkar
        relaterade tabeller vid borttagning av tex en user*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserTask>()
                .HasOne(ut => ut.user)
                .WithMany(u => u.UserTasks)
                .HasForeignKey(ut => ut.userID_FK)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserTask>()
                .HasOne(ut => ut.task)
                .WithMany(t => t.UserTasks)
                .HasForeignKey(ut => ut.taskID_FK)
                .OnDelete(DeleteBehavior.NoAction); // Viktigt: Endast EN av dessa får ha Cascade
        }

        //Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Database=TeamAppDB;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;

        //Agerar som en bro mot tabellen i databasen
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<LogEntry> LogEntrys { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }

    }
}
