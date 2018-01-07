using liteFTP.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;


namespace liteFTP.ViewModels
{
    public class DirectoryItemVM : BaseViewModel
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

        public ObservableCollection<DirectoryItemVM> Children { get; set; }

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

        public DirectoryItemVM(string path, DirectoryItems type)
        {
            Path = path;
            Type = type;

            ExpandCommand = new RelayCommand(ExpandDirectory);

            ClearChildren();
        }

        public void ExpandDirectory()
        {
            if (Type == DirectoryItems.File)
                return;

            var children = DirectoryManager.GetAllItems(Path);
            Children = new ObservableCollection<DirectoryItemVM>(
                                children.Select(content => new DirectoryItemVM(content.Path, content.Type)));
        }


        public void ClearChildren()
        {
            Children = new ObservableCollection<DirectoryItemVM>();

            if (Type != DirectoryItems.File)
                Children.Add(null);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
