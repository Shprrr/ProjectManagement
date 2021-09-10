using System;

namespace ProjectManagement.Models
{
    public class ProjectDocument
    {
        public ProjectDocument(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; set; }
        public string Filename { get; set; }
    }
}
