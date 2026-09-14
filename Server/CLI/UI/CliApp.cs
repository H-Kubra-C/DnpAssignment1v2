using Entities;
using RepositoryContracts;
using CLI.UI.ManageUsers;
using CLI.UI.ManagePosts;
using CLI.UI.ManageComments;

namespace CLI.UI;

public class CliApp
{
    private CreateUserView createUserView;
    private CreatePostView createPostView;
    private AddCommentView addCommentView;
    private PostOverviewView postOverviewView;
    private SpecificPostView specificPostView;

    public CliApp(IUserRepository userRepository,
        IPostRepository postRepository, ICommentRepository commentRepository)
    {
        createUserView = new CreateUserView(userRepository);
        createPostView = new CreatePostView(postRepository);
        addCommentView = new AddCommentView(commentRepository);
        postOverviewView = new PostOverviewView(postRepository);
        specificPostView =
            new SpecificPostView(postRepository, commentRepository);
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
                    await createUserView.CreateUserAsync();
                    break;

                case "2":
                    await createPostView.CreatePostAsync();
                    break;

                case "3":
                    await addCommentView.AddCommentAsync();
                    break;

                case "4":
                    postOverviewView.ViewPosts();
                    break;

                case "5":
                    await specificPostView.ViewSpecificPostAsync();
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
}