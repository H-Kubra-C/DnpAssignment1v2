namespace ApiContracts;

public class PostDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    
    public string Username { get; set; } = string.Empty;
    public string Title { get; set; }
    public string Body { get; set; }
    public List<CommentDto> Comments { get; set; } = new();
}