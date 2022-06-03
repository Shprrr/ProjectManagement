using Newtonsoft.Json;

namespace ProjectManagement.Models
{
    public abstract class ItemNode
    {
        public string Title { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public virtual bool CanRemove => true;

        [JsonIgnore]
        public abstract ItemNode Parent { get; set; }
        public abstract void AddChild(ItemNode node);
        public abstract ItemNode[] GetChildren();
        public abstract void RemoveChild(ItemNode node);
    }
}
