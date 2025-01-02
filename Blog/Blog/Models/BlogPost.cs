namespace Blog.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public IEnumerable<Comment>? Comments { get; set; }
    }
}
