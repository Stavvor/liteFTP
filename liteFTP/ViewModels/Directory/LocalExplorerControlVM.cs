using liteFTP.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace liteFTP.ViewModels
{
    public class LocalExplorerControlVM : BaseViewModel
    {
        private string _currentPath = ""; //TODO

        private DirectoryItemVM _selectedItem;

        public string CurrentPath {
            get {
                return _currentPath;
            }
            set {
                _currentPath = value;

                var allItems = DirectoryManager.GetAllItems(value);

                CurrentFolderItems = new ObservableCollection<DirectoryItemVM>(
                    allItems.Select(item=> new DirectoryItemVM(item.Path, item.Type))
                    );
            }
        }
        public ObservableCollection<DirectoryItemVM> Items { get; set; }

        public ObservableCollection<DirectoryItemVM> CurrentFolderItems { get; set; }

        public DirectoryItemVM SelectedItem { get { return _selectedItem; } set { _selectedItem = value; CurrentPath = SelectedItem.Path; } }

        public LocalExplorerControlVM()
        { 
            var children = DirectoryManager.GetDrives();

            Items = new ObservableCollection<DirectoryItemVM>(
                children.Select(drive => new DirectoryItemVM(drive.Path, DirectoryItems.Drive)
                ));
        }


    }
}
