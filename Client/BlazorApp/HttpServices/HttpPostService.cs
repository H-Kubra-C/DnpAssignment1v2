using ApiContracts;

namespace BlazorApp.HttpServices;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        HttpResponseMessage response =
            await client.PostAsJsonAsync("api/Posts", request);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode},{content}");
        }

        PostDto? post = await response.Content.ReadFromJsonAsync<PostDto>();
        return post ?? throw new Exception("Could not create post");
    }
    
    
    public async Task<ICollection<PostDto>> GetPostsAsync()
    {
        HttpResponseMessage response =
            await client.GetAsync("api/Posts");

        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        ICollection<PostDto>? posts =
            await response.Content.ReadFromJsonAsync<List<PostDto>>();

        return posts ?? [];
    }

    
    public async Task<PostDto> GetPostAsync(int id)
    {
        HttpResponseMessage response =
            await client.GetAsync($"api/Posts/{id}");

        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        PostDto? post =
            await response.Content.ReadFromJsonAsync<PostDto>();

        return post ?? throw new Exception("Could not find post");
    }

}