using System;
using System.IO;
using Newtonsoft.Json;

namespace ProjectManagement.Models
{
    public class ProjectDocument
    {
        public ProjectDocument(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; set; }
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool CanBeSaved => !string.IsNullOrWhiteSpace(Filename);

        public void Save()
        {
            if (!CanBeSaved) throw new InvalidOperationException("Cannot be saved.");

            File.WriteAllText(Filename, JsonConvert.SerializeObject(this));
        }

        public static ProjectDocument Load(string filename)
        {
            var document = JsonConvert.DeserializeObject<ProjectDocument>(File.ReadAllText(filename));
            document.Filename = filename;
            return document;
        }
    }
}
