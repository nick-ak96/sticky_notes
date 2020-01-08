namespace api.Models
{
    public class UserNoteSharing
    {
        public UserNoteSharing()
        {
        }

        public UserNoteSharing(NoteSharing ns, User u)
        {
            UserId = ns.CollaboratorId;
            NoteId = ns.NoteId;
            AccessType = ns.AccessType;
            UserDetails = u;
        }

        public long UserId { get; set; }

        public long NoteId { get; set; }

        public NoteAccessType AccessType { get; set; }

        public User UserDetails { get; set; }
    }

    public class OrgNoteSharing
    {
        public OrgNoteSharing()
        {
        }

        public OrgNoteSharing(NoteSharing ns, Organization o)
        {
            OrganizationId = ns.CollaboratorId;
            NoteId = ns.NoteId;
            AccessType = ns.AccessType;
            OrganizationDetails = o;
        }

        public long OrganizationId { get; set; }

        public long NoteId { get; set; }

        public NoteAccessType AccessType { get; set; }

        public Organization OrganizationDetails { get; set; }
    }

    public class NoteSharing
    {
        public long NoteId { get; set; }

        public long CollaboratorId { get; set; }

        public NoteAccessType AccessType { get; set; }
    }
}
