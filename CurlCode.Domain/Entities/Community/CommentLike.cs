using CurlCode.Domain.Common;
using CurlCode.Domain.Entities.Identity;

namespace CurlCode.Domain.Entities.Community;

public class CommentLike : BaseEntity
{
    public int CommentId { get; set; }
    public string UserId { get; set; } = string.Empty;
    // Navigation properties
    public Comment Comment { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}
