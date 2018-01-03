using liteFTP.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace liteFTP.ViewModels
{
    public class LocalExplorerControlVM : BaseViewModel
    {
        public string CurrentPath {
            get {
                return $"C:\\Users\\Stavor\\Documents\\Visual Studio 2017\\Projects\\liteFTP\\liteFTP\\bin\\Debug"; //TODO 
            }
            set {
                CurrentPath = value;
            }
        }
        public ObservableCollection<DirectoryItemVM> Items { get; set; }

        public LocalExplorerControlVM()
        { 
            var children = DirectoryManager.GetDrives();

            Items = new ObservableCollection<DirectoryItemVM>(
                children.Select(drive => new DirectoryItemVM(drive.Path, DirectoryItems.Drive)));
        }


    }
}
