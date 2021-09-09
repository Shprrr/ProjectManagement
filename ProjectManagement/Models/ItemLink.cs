using QuikGraph;

namespace ProjectManagement.Models
{
    internal class ItemLink : IEdge<ItemNode>
    {
        public ItemNode Source { get; set; }

        public ItemNode Target { get; set; }
    }
}
