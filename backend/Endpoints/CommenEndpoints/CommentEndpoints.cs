using TeamApp.DTOs.CommentDTO;
using TeamApp.Services;

namespace TeamApp.Endpoints.CommenEndpoints
{
    public class CommentEndpoints
    {
        public static void MessageEndpoints(WebApplication app)
        {
            //Listar alla kommentarer
            app.MapGet("/messages", async (CommentService commentService) =>
            {
                var messages = await commentService.GetAllComments();

                if (messages == null) { return Results.NotFound("Inga kommentarer hittades"); }

                return Results.Ok(messages);
            });

            //Skapar en ny kommentar
            app.MapPost("/messagescreate", async (CreateCommentDTO newComment, CommentService commentService) =>
            {
                var createdMessage = await commentService.CreateComment(newComment);

                if (createdMessage == null) { return Results.BadRequest("Går ej att skapa en ny kommentar"); }

                return Results.Ok(createdMessage);
            });

            //Uppdaterar kommentar
            app.MapPut("/messageupdate {id}", async (CommentService commentService, UpdateCommentDTO commentUpdate, int id) =>
            {
                if (commentUpdate.CommentID != id)
                {
                    return Results.BadRequest("ID:t som du har angett finns inte!");
                }

                var result = await commentService.UpdateComment(commentUpdate);

                if (result == null) { return Results.NotFound("Användaren hittades inte"); }

                return Results.Ok(result);
            });

            //Tar bort kommentar
            app.MapDelete("/messagedelete {id}", async (CommentService commentService, int id) =>
            {
                var result = await commentService.DeleteUser(id);
                if (!result) { return Results.NotFound("Användaren hittades inte"); }

                return Results.Ok(result);

            });

        }
    }
}
