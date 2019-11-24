namespace api.Models
{
    public class Note
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public bool IsPublic { get; set; }
    }
}
