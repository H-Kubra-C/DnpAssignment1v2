using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task CreateUserAsync()
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

        User created = await userRepository.AddAsync(user);
        Console.WriteLine($"User created successfully. ID:{created.Id}");
    }
}