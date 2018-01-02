using liteFTP.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace liteFTP
{
    public class DirectoryViewModel : BaseViewModel
    {

        public ObservableCollection<DirectoryItemViewModel> Items { get; set; }

        public DirectoryViewModel()
        {

            var children = DirectoryManager.GetDrives();

            this.Items = new ObservableCollection<DirectoryItemViewModel>(
                children.Select(drive => new DirectoryItemViewModel(drive.Path, DirectoryItems.Drive)));
        }


    }
}
