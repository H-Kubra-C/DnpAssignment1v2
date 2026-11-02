using System.Text.Json;
using ApiContracts;

namespace BlazorApp.HttpServices;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        HttpResponseMessage response =
            await client.PostAsJsonAsync("api/Users", request);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode},{content}");
        }

        UserDto? user = await response.Content.ReadFromJsonAsync<UserDto>();
        return user ?? throw new Exception("Could not create user");
    }
}