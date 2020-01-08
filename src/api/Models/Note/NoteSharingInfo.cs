using System.Collections.Generic;

namespace api.Models
{
    public class NoteSharingInfo
    {
        public NoteSharingInfo(long noteId)
        {
            NoteId = noteId;
            IsPublic = false;
            Users = new List<UserNoteSharing>();
            Organizations = new List<OrgNoteSharing>();
        }

        public long NoteId { get; set; }

        public bool IsPublic { get; set; }

        public IEnumerable<UserNoteSharing> Users { get; set; }

        public IEnumerable<OrgNoteSharing> Organizations { get; set; }
    }
}
