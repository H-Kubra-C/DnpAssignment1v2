using CLI.UI;
using InMemoryRepositories;

UserInMemoryRepository userRepository = new UserInMemoryRepository();
PostInMemoryRepository postRepository = new PostInMemoryRepository();
CommentInMemoryRepository commentRepository = new CommentInMemoryRepository();


CliApp app = new CliApp(userRepository, postRepository, commentRepository);

await app.StartAsync();