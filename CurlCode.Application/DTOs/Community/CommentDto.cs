namespace CurlCode.Application.DTOs.Community;

public class CommentDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? UserName { get; set; }
}


