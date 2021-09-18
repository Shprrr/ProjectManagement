using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProjectManagement.Models
{
    public class Goal : ItemNode
    {
        private readonly List<Goal> goals = new();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Supprimer les membres privés non utilisés", Justification = "For Deserializing")]
        [JsonConstructor]
        private Goal(Goal[] goals)
        {
            this.goals.AddRange(goals);
        }

        public Goal(string title) => Title = title ?? throw new ArgumentNullException(nameof(title));

        public Goal[] Goals => goals.ToArray();

        public override void AddChild(ItemNode node)
        {
            goals.Add((Goal)node);
        }

        public override ItemNode[] GetChildren() => Goals;
    }
}
