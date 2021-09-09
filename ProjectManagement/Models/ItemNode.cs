namespace ProjectManagement.Models
{
    internal class ItemNode
    {
        public ItemNode(string title)
        {
            Title = title;
        }

        public string Title { get; set; }
        public string Description { get; set; }
    }
}
