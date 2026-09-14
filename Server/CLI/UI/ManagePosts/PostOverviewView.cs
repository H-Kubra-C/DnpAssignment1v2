using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class PostOverviewView
{
    private readonly IPostRepository postRepository;

    public PostOverviewView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public void ViewPosts()
    {
        IQueryable<Post> posts = postRepository.GetManyAsync();

        foreach (Post post in posts)
        {
            Console.WriteLine($"{post.Id}: {post.Title}");
        }
    }
}