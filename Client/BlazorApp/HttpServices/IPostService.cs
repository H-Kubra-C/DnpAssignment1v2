using ApiContracts;

namespace BlazorApp.HttpServices;

public interface IPostService
{
    Task<PostDto> AddPostAsync(CreatePostDto request);
    Task<ICollection<PostDto>> GetPostsAsync();
    Task<PostDto> GetPostAsync(int id);
}
