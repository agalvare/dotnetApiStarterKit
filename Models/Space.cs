using System;

namespace SpaceAPI.Models
{
    public class Space
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string DefaultFocusMode { get; set; } = string.Empty;
        public string Projects { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Private { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastAccessedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UploadedFiles { get; set; } = string.Empty;
        public string SystemMessage { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
    }
}