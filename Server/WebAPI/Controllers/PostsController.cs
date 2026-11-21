using ApiContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using Entities;

namespace WebAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;
    private readonly ICommentRepository commentRepo;
    private readonly IUserRepository userRepo;

    public PostsController(
        IPostRepository postRepo,
        ICommentRepository commentRepo, IUserRepository userRepo)
    {
        this.postRepo = postRepo;
        this.commentRepo = commentRepo;
        this.userRepo = userRepo;
    }

    // POST - Opret et post
    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost(
        [FromBody] CreatePostDto request)
    {
        Post post = new Post(request.Title, request.Body, request.UserId);

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
        IQueryable<Post> queryForPost = postRepo.GetManyAsync()
            .Where(p => p.Id == id);

        queryForPost = queryForPost.Include(p => p.User);
        queryForPost = queryForPost.Include(p => p.Comments);

        PostDto? dto = await queryForPost
            .Select(post => new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                Username = post.User.Username,
                Comments = post.Comments.Select(c => new CommentDto
                {
                    Id = c.Id,
                    Body = c.Body,
                    UserId = c.UserId,
                    PostId = c.PostId
                }).ToList()
            })
            .FirstOrDefaultAsync();

        return dto == null ? NotFound() : Ok(dto);
    }


    // GET - Hent alle posts eller filtrer
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts(
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

        var postDtos = await posts.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Body = p.Body,
            UserId = p.UserId
        }).ToListAsync();

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