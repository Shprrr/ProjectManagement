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
        private const string ProjectExtension = ".prj";
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
            if (!BeforeNewDocument()) return;

            NewProjectWindow newProjectWindow = new();
            if (newProjectWindow.ShowDialog().GetValueOrDefault())
            {
                viewModel.OpenedDocument = new(newProjectWindow.ProjectName);
                viewModel.IsDirty = false;
                ShowOpenedDocument();
            }
        }

        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!BeforeNewDocument()) return;

            OpenFileDialog openFileDialog = new()
            {
                DefaultExt = ProjectExtension,
                Filter = $"Project file (*{ProjectExtension})|*{ProjectExtension}|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                viewModel.OpenedDocument = ProjectDocument.Load(openFileDialog.FileName);
                viewModel.IsDirty = false;
                ShowOpenedDocument();
            }
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = viewModel.OpenedDocument != null && viewModel.IsDirty;
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e) => SaveDocument();

        private void ExitCommand_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Returns if we accept to load the new document.
        /// </summary>
        /// <returns></returns>
        private bool BeforeNewDocument()
        {
            if (viewModel.IsDirty)
                switch (MessageBox.Show("Do you want to save the changes ?", "Unsaved changes", MessageBoxButton.YesNoCancel))
                {
                    case MessageBoxResult.Cancel:
                        return false;

                    case MessageBoxResult.Yes:
                        return SaveDocument();
                }

            return true;
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

        private bool SaveDocument()
        {
            if (!viewModel.OpenedDocument.CanBeSaved)
            {
                SaveFileDialog saveFileDialog = new()
                {
                    FileName = $"{viewModel.OpenedDocument.Name}{ProjectExtension}",
                    DefaultExt = ProjectExtension,
                    Filter = $"Project file (*{ProjectExtension})|*{ProjectExtension}|All files (*.*)|*.*",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };
                if (!saveFileDialog.ShowDialog().GetValueOrDefault()) return false;
                viewModel.OpenedDocument.Filename = saveFileDialog.FileName;
            }

            viewModel.OpenedDocument.Save();
            viewModel.IsDirty = false;
            return true;
        }
    }
}
