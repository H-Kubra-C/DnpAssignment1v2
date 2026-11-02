using ApiContracts;

namespace BlazorApp.HttpServices;

public interface ICommentService
{
    Task<CommentDto> AddCommentAsync(CreateCommentDto request);

}