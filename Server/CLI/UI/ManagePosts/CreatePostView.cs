using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;

    public CreatePostView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public async Task CreatePostAsync()
    {
        Console.WriteLine();
        Console.Write("Title: ");
        string? title = Console.ReadLine();

        Console.Write("Body: ");
        string? body = Console.ReadLine();

        Console.Write("User Id: ");
        int userId = Convert.ToInt32(Console.ReadLine());

        if (title is null || body is null)
        {
            Console.WriteLine("Title or body cannot be null.");
            return;
        }

        Post post = new Post
        {
            Title = title,
            Body = body,
            UserId = userId
        };

        Post created = await postRepository.AddAsync(post);
        Console.WriteLine($"Post created successfully. ID: {created.Id}");
    }
}