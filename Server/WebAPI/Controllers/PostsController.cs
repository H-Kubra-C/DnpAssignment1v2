
using ApiContracts;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;

namespace WebAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;
    private readonly ICommentRepository commentRepo;

    public PostsController(
        IPostRepository postRepo,
        ICommentRepository commentRepo)
    {
        this.postRepo = postRepo;
        this.commentRepo = commentRepo;
    }

    // POST - Opret et post
    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost(
        [FromBody] CreatePostDto request)
    {
        Post post = new Post
        {
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserId
        };

        Post created = await postRepo.AddAsync(post);

        PostDto dto = new PostDto
        {
            Id = created.Id,
            Title = created.Title,
            Body = created.Body,
            UserId = created.UserId
        };

        return Created($"/api/Posts/{dto.Id}", dto);
    }

    // GET - Hent et post med kommentarer
    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetPost(
        [FromRoute] int id)
    {
        try
        {
            Post post = await postRepo.GetSingleAsync(id);

            var comments = commentRepo.GetManyAsync()
                .Where(c => c.PostId == id)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Body = c.Body,
                    UserId = c.UserId,
                    PostId = c.PostId
                })
                .ToList();

            PostDto dto = new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                Comments = comments
            };

            return Ok(dto);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    // GET - Hent alle posts eller filtrer
    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetPosts(
        [FromQuery] string? titleContains,
        [FromQuery] int? userId)
    {
        var posts = postRepo.GetManyAsync();

        if (!string.IsNullOrWhiteSpace(titleContains))
        {
            posts = posts.Where(p =>
                p.Title.ToLower().Contains(titleContains.ToLower()));
        }

        if (userId.HasValue)
        {
            posts = posts.Where(p => p.UserId == userId.Value);
        }

        var postDtos = posts.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Body = p.Body,
            UserId = p.UserId
        }).ToList();

        return Ok(postDtos);
    }

    // PUT - Opdater et post
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(
        [FromRoute] int id,
        [FromBody] CreatePostDto request)
    {
        try
        {
            Post post = await postRepo.GetSingleAsync(id);

            post.Title = request.Title;
            post.Body = request.Body;
            post.UserId = request.UserId;

            await postRepo.UpdateAsync(post);

            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    // DELETE - Slet et post
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(
        [FromRoute] int id)
    {
        try
        {
            await postRepo.DeleteAsync(id);

            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}
