using System;

namespace api.Models
{
    public class Note
    {
        public Note()
        {
        }

        public Note(NoteCreate note, long creator)
        {
            Content = note.Content;
            Language = note.Language;
            Color = note.Color;
            CreatedBy = creator;
            InsertDate = DateTime.UtcNow;
            LastModified = DateTime.UtcNow;
        }

        public long Id { get; set; }

        public string Content { get; set; }

        public string Language { get; set; }

        public int Color { get; set; }

        public bool IsPinned { get; set; }

        public bool IsPublic { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime LastModified { get; set; }

        public long CreatedBy { get; set; }

        public string Username { get; set; }

        public void Patch(NoteUpdate patch)
        {
            Content = patch.Content ?? Content;
            Language = patch.Language ?? Language;
            Color = patch.Color ?? Color;
        }
    }
}
