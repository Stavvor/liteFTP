using liteFTP.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;


namespace liteFTP
{
    public class DirectoryItemViewModel : BaseViewModel
    {
        public DirectoryItems Type { get; set; }

        public string Path { get; set; }

        public string Name { get {
                if (Type == DirectoryItems.Drive)
                {
                    return Path;
                }
                else return DirectoryManager.GetNameFromPath(Path);
            }
        }

        public ObservableCollection<DirectoryItemViewModel> Children { get; set; }

        public bool Expandable { get { return Type != DirectoryItems.File; } }


        public bool IsExpanded
        {
            get
            {
                return Children?.Count(f=>f != null) > 0;
            }
            set
            {
                if (value) ExpandDirectory();
                else ClearChildren();
            }
        }

        public ICommand ExpandCommand { get; set; }

        public DirectoryItemViewModel(string path, DirectoryItems type)
        {
            Path = path;
            Type = type;

            ExpandCommand = new RelayCommand(ExpandDirectory);

            ClearChildren();
        }

        private void ExpandDirectory()
        {
            if (Type == DirectoryItems.File)
                return;

            var children = DirectoryManager.GetAllItems(Path);
            Children = new ObservableCollection<DirectoryItemViewModel>(
                                children.Select(content => new DirectoryItemViewModel(content.Path, content.Type)));
        }


        private void ClearChildren()
        {
            Children = new ObservableCollection<DirectoryItemViewModel>();

            if (Type != DirectoryItems.File)
                Children.Add(null);
        }
    }
}
