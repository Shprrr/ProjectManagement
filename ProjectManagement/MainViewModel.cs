using System.ComponentModel;
using System.Runtime.CompilerServices;
using ProjectManagement.Models;

namespace ProjectManagement
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        private ProjectDocument openedDocument;
        private bool isDirty;

        public ProjectDocument OpenedDocument { get => openedDocument; set => SetProperty(ref openedDocument, value); }

        public bool IsDirty { get => isDirty; set => SetProperty(ref isDirty, value); }
    }
}