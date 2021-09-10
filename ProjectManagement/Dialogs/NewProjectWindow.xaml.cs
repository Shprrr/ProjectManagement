using System.Windows;

namespace ProjectManagement.Dialogs
{
    /// <summary>
    /// Logique d'interaction pour NewProjectWindow.xaml
    /// </summary>
    public partial class NewProjectWindow : Window
    {
        public string ProjectName => txtProjectName.Text;

        public NewProjectWindow()
        {
            InitializeComponent();
        }

        private void CreateProject_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
