using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TeamApp.Data;
using TeamApp.DTOs.CommentDTO;
using TeamApp.Models;

namespace TeamApp.Services
{
    public class CommentService
    {
        private readonly TeamDBContext context;

        //Dependancy injection
        //Bättrestruktur
        public CommentService(TeamDBContext _context)
        {
            context = _context;
        }

        public async Task<List<CommentDTO>> GetAllComments()
        {
            var commentList = await context.Comments.Select(c => new CommentDTO
            {
                CommentID = c.CommentID,
                Text = c.Text,
                CreatedAt = c.CreatedAt
            }).ToListAsync();
            return commentList;
        }

        public async Task<CommentDTO?> GetCommentByID(int messageID)
        {
            var message = await context.Comments.Where(c => c.CommentID == messageID).Select(c => new CommentDTO
            {
                Text = c.Text,
                CreatedAt = c.CreatedAt

            }).FirstOrDefaultAsync();
            return message;
        }

        public async Task<CommentDTO> CreateComment(CreateCommentDTO newComment)
        {
            var validationContext = new ValidationContext(newComment);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(newComment, validationContext, validationResults, true);

            if (!isValid)
            {
                throw new ValidationException("Valideringen misslyckades för den nya användaren");
            }

            var comment = new Comment
            {
                Text = newComment.Text,
            };

            context.Comments.Add(comment);
            await context.SaveChangesAsync();

            return new CommentDTO
            {
                Text = newComment.Text

            };
        }

        public async Task<CommentDTO?> UpdateComment(UpdateCommentDTO commentUpdate)
        {
            var message = await context.Comments.FirstOrDefaultAsync(c => c.CommentID == commentUpdate.CommentID);
            if (message == null) { return null; }

            message.Text = commentUpdate.Text;

            await context.SaveChangesAsync();

            return new CommentDTO
            {
                Text = message.Text
            };
        }

        public async Task<bool> DeleteUser(int id)
        {
            var message = await context.Comments.FirstOrDefaultAsync(c => c.CommentID == id);

            if (message == null) { return false; }

            context.Comments.Remove(message);
            await context.SaveChangesAsync();
            return true;
        }

    }
}
