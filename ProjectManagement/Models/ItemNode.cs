namespace ProjectManagement.Models
{
    public abstract class ItemNode
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public abstract ItemNode Parent { get; set; }
        public abstract void AddChild(ItemNode node);
        public abstract ItemNode[] GetChildren();
        public abstract void RemoveChild(ItemNode node);
    }
}
