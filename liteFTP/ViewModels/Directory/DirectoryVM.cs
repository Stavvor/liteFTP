using liteFTP.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace liteFTP.ViewModels
{
    public class DirectoryVM : BaseViewModel
    {

        public ObservableCollection<DirectoryItemVM> Items { get; set; }

        public DirectoryVM()
        {

            var children = DirectoryManager.GetDrives();

            this.Items = new ObservableCollection<DirectoryItemVM>(
                children.Select(drive => new DirectoryItemVM(drive.Path, DirectoryItems.Drive)));
        }


    }
}
