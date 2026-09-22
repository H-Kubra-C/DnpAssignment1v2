using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<User> AddAsync(User user)
    {
        List<User> users = await LoadUsersAsync();

        user.Id = users.Any()
            ? users.Max(u => u.Id) + 1
            : 1;

        users.Add(user);

        await SaveUsersAsync(users);

        return user;
    }

    public async Task UpdateAsync(User user)
    {
        List<User> users = await LoadUsersAsync();

        User? existingUser =
            users.SingleOrDefault(u => u.Id == user.Id);

        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.Id}' not found");
        }

        users.Remove(existingUser);
        users.Add(user);

        await SaveUsersAsync(users);
    }

    public async Task DeleteAsync(int id)
    {
        List<User> users = await LoadUsersAsync();

        User? userToRemove =
            users.SingleOrDefault(u => u.Id == id);

        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }

        users.Remove(userToRemove);

        await SaveUsersAsync(users);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        List<User> users = await LoadUsersAsync();

        User? user =
            users.SingleOrDefault(u => u.Id == id);

        if (user is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }

        return user;
    }

    public IQueryable<User> GetManyAsync()
    {
        List<User> users = LoadUsersAsync().Result;

        return users.AsQueryable();
    }

    private async Task<List<User>> LoadUsersAsync()
    {
        string usersAsJson =
            await File.ReadAllTextAsync(filePath);

        List<User>? users =
            JsonSerializer.Deserialize<List<User>>(usersAsJson);

        return users ?? new List<User>();
    }

    private async Task SaveUsersAsync(List<User> users)
    {
        string usersAsJson = JsonSerializer.Serialize(
            users,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        await File.WriteAllTextAsync(filePath, usersAsJson);
    }
}