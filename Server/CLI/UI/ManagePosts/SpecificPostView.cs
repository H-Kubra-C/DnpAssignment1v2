using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SpecificPostView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;

    public SpecificPostView(
        IPostRepository postRepository,
        ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

    public async Task ViewSpecificPostAsync()
    {
        Console.WriteLine();

        Console.Write("Post Id: ");
        int postId = Convert.ToInt32(Console.ReadLine());

        Post post = await postRepository.GetSingleAsync(postId);

        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Body: {post.Body}");
        Console.WriteLine("Comments:");

        IQueryable<Comment> comments = commentRepository.GetManyAsync();

        foreach (Comment comment in comments)
        {
            if (comment.PostId == postId)
            {
                Console.WriteLine(comment.Body);
            }
        }
    }
}