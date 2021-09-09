using System.Linq;
using System.Windows;
using GraphShape.Controls;
using ProjectManagement.Models;
using QuikGraph;

namespace ProjectManagement
{
    internal class GraphLayout : GraphLayout<ItemNode, ItemLink, IBidirectionalGraph<ItemNode, ItemLink>>
    {
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            BidirectionalGraph<ItemNode, ItemLink> graph = new();
            graph.AddVerticesAndEdge(new ItemLink { Source = new ItemNode("Project 1"), Target = new ItemNode("Target") { Description = "Description of the first target." } });
            graphCanvas.Graph = graph;

            cboLayoutChoices.ItemsSource = graphCanvas.LayoutAlgorithmFactory.AlgorithmTypes.ToList();
        }
    }
}
