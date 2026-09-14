using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private  IUserRepository userRepository;
    private  IPostRepository postRepository;
    private  ICommentRepository commentRepository;

    public CliApp(IUserRepository userRepository,
        IPostRepository postRepository, ICommentRepository commentRepository)
    {
        this.userRepository = userRepository;
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;


    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Welcome");
            Console.WriteLine("1: Create user");
            Console.WriteLine("2: Create post");
            Console.WriteLine("3: Add comment");
            Console.WriteLine("4: View posts");
            Console.WriteLine("5: View specific post");
            Console.WriteLine("0: Exit");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await CreateUserAsync();
                    break;
                
                case "2":
                    await CreatePostAsync();
                    break;
                
                case "3":
                    await AddCommentAsync();
                    break;
                
                case "4":
                    ViewPosts();
                    break;
                
                case "5":
                    await ViewSpecificPostAsync();
                    break;
                
                case "0":
                    Console.WriteLine("Exit");
                    return;

                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }
    }

    private async Task CreateUserAsync()
    {
        Console.WriteLine();
        Console.Write("Username: ");
        string? username = Console.ReadLine();

        Console.Write("Password: ");
        string? password = Console.ReadLine();

        if (username is null || password is null)
        {
            Console.WriteLine("Username or password cannot be null.");
            return;
        }

        User user = new User
        {
            Username = username,
            Password = password
        };

        await userRepository.AddAsync(user);

        Console.WriteLine("User created!");
    }
    private async Task CreatePostAsync()
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

        await postRepository.AddAsync(post);

        Console.WriteLine("Post created!");
    }
    
    private async Task AddCommentAsync()
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

        await commentRepository.AddAsync(comment);

        Console.WriteLine("Comment added!");
        
        
    }
    
    private void ViewPosts()
    {
        IQueryable<Post> posts = postRepository.GetManyAsync();

        foreach (Post post in posts)
        {
            Console.WriteLine($"{post.Id}: {post.Title}");
        }
    }
    
    private async Task ViewSpecificPostAsync()
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


