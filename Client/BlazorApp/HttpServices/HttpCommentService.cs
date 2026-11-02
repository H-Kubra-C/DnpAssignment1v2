using System.Net.Http.Json;
using ApiContracts;

namespace BlazorApp.HttpServices;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<CommentDto> AddCommentAsync(CreateCommentDto request)
    {
        HttpResponseMessage response =
            await client.PostAsJsonAsync("api/Comments", request);

        response.EnsureSuccessStatusCode();

        CommentDto? comment =
            await response.Content.ReadFromJsonAsync<CommentDto>();

        return comment!;
    }
}