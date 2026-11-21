namespace Entities;

public class User
{
    
    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }
    
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    private User()
    {
    }
    // for EFC
}