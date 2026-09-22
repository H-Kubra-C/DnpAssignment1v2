using CLI.UI;
using FileRepositories;
using RepositoryContracts;

Console.WriteLine("Starting the CLI application...");
IUserRepository userRepository = new UserFileRepository();
IPostRepository postRepository = new PostFileRepository();
ICommentRepository commentRepository = new CommentFileRepository();

CliApp app = new CliApp(userRepository, postRepository, commentRepository);
await app.StartAsync();