using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ProjectManagement.Models
{
    public class ProjectDocument : ItemNode
    {
        private readonly List<Goal> goals = new();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Supprimer les membres privés non utilisés", Justification = "For Deserializing")]
        [JsonConstructor]
        private ProjectDocument(Goal[] goals)
        {
            this.goals.AddRange(goals);
        }

        public ProjectDocument(string name)
        {
            Title = name ?? throw new ArgumentNullException(nameof(name));
        }

        public Goal[] Goals => goals.ToArray();
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
            var document = JsonConvert.DeserializeObject<ProjectDocument>(File.ReadAllText(filename),
                new JsonSerializerSettings { ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor });
            document.Filename = filename;
            return document;
        }

        public override ItemNode Parent { get => null; set => throw new InvalidOperationException(); }

        public override void AddChild(ItemNode node)
        {
            node.Parent = this;
            goals.Add((Goal)node);
        }

        public override ItemNode[] GetChildren() => Goals;

        public override void RemoveChild(ItemNode node)
        {
            goals.Remove((Goal)node);
            node.Parent = null;
        }
    }
}
