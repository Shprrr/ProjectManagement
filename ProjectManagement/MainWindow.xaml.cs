using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GraphShape.Controls;
using Microsoft.Win32;
using ProjectManagement.Dialogs;
using ProjectManagement.Models;
using QuikGraph;

namespace ProjectManagement
{
    internal class GraphLayout : GraphLayout<ItemNode, ItemLink, BidirectionalGraph<ItemNode, ItemLink>>
    {
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel viewModel = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;

            cboLayoutChoices.ItemsSource = graphCanvas.LayoutAlgorithmFactory.AlgorithmTypes.ToList();
        }

        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (viewModel.IsDirty)
                switch (MessageBox.Show("Do you want to save the changes ?", "Unsaved changes", MessageBoxButton.YesNoCancel))
                {
                    case MessageBoxResult.Cancel:
                        return;

                    case MessageBoxResult.Yes:
                        //TODO: Save
                        break;
                }

            NewProjectWindow newProjectWindow = new();
            if (newProjectWindow.ShowDialog().GetValueOrDefault())
            {
                viewModel.OpenedDocument = new(newProjectWindow.ProjectName);
                viewModel.IsDirty = false;
                ShowOpenedDocument();
            }
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = viewModel.OpenedDocument != null && viewModel.IsDirty;
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!viewModel.OpenedDocument.CanBeSaved)
            {
                SaveFileDialog saveFileDialog = new();
                saveFileDialog.FileName = $"{viewModel.OpenedDocument.Name}.prj";
                saveFileDialog.DefaultExt = ".prj";
                saveFileDialog.Filter = "Project file (*.prj)|*.prj|All files (*.*)|*.*";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (!saveFileDialog.ShowDialog().GetValueOrDefault()) return;
                viewModel.OpenedDocument.Filename = saveFileDialog.FileName;
            }

            viewModel.OpenedDocument.Save();
        }

        private void ExitCommand_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void ShowOpenedDocument()
        {
            if (viewModel.OpenedDocument == null)
            {
                graphCanvas.Graph.Clear();
                return;
            }

            BidirectionalGraph<ItemNode, ItemLink> graph = new();
            graph.AddVertex(new ItemNode(viewModel.OpenedDocument.Name));
            graphCanvas.Graph = graph;
        }
    }
}
