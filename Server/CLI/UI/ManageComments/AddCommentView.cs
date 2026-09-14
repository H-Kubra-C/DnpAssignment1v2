using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class AddCommentView
{
    private readonly ICommentRepository commentRepository;

    public AddCommentView(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    public async Task AddCommentAsync()
    {
        Console.WriteLine();

        Console.Write("Body: ");
        string? body = Console.ReadLine();

        Console.Write("User Id: ");
        int userId = Convert.ToInt32(Console.ReadLine());

        Console.Write("Post Id: ");
        int postId = Convert.ToInt32(Console.ReadLine());

        if (body is null)
        {
            Console.WriteLine("Body cannot be null.");
            return;
        }

        Comment comment = new Comment
        {
            Body = body,
            UserId = userId,
            PostId = postId
        };
        Comment created = await commentRepository.AddAsync(comment);
        Console.WriteLine($"Comment added successfully. ID: {created.Id}");
    }
}