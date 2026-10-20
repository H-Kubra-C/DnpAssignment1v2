
using ApiContracts;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;

namespace WebAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;

    public CommentsController(ICommentRepository commentRepo)
    {
        this.commentRepo = commentRepo;
    }

    // POST - Opret en kommentar
    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment(
        [FromBody] CreateCommentDto request)
    {
        Comment comment = new Comment
        {
            Body = request.Body,
            UserId = request.UserId,
            PostId = request.PostId
        };

        Comment created = await commentRepo.AddAsync(comment);

        CommentDto dto = new CommentDto
        {
            Id = created.Id,
            Body = created.Body,
            UserId = created.UserId,
            PostId = created.PostId
        };

        return Created($"/api/Comments/{dto.Id}", dto);
    }

    // GET - Hent en kommentar
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetComment(
        [FromRoute] int id)
    {
        try
        {
            Comment comment = await commentRepo.GetSingleAsync(id);

            CommentDto dto = new CommentDto
            {
                Id = comment.Id,
                Body = comment.Body,
                UserId = comment.UserId,
                PostId = comment.PostId
            };

            return Ok(dto);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    // GET - Hent alle kommentarer eller filtrer
    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetComments(
        [FromQuery] int? postId,
        [FromQuery] int? userId)
    {
        var comments = commentRepo.GetManyAsync();

        if (postId.HasValue)
        {
            comments = comments.Where(c => c.PostId == postId.Value);
        }

        if (userId.HasValue)
        {
            comments = comments.Where(c => c.UserId == userId.Value);
        }

        var commentDtos = comments.Select(c => new CommentDto
        {
            Id = c.Id,
            Body = c.Body,
            UserId = c.UserId,
            PostId = c.PostId
        }).ToList();

        return Ok(commentDtos);
    }

    // PUT - Opdater en kommentar
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(
        [FromRoute] int id,
        [FromBody] CreateCommentDto request)
    {
        try
        {
            Comment comment = await commentRepo.GetSingleAsync(id);

            comment.Body = request.Body;
            comment.UserId = request.UserId;
            comment.PostId = request.PostId;

            await commentRepo.UpdateAsync(comment);

            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    // DELETE - Slet en kommentar
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(
        [FromRoute] int id)
    {
        try
        {
            await commentRepo.DeleteAsync(id);

            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}
