using System.Collections.ObjectModel;

namespace liteFTP.ViewModels
{
    public class RemoteExplorerControlVM : BaseViewModel
    {
        public string CurrentPath { get { return $"ftp://placeholder\\someDirectory"; } set { CurrentPath = value; } }
        public ObservableCollection<DirectoryItemVM> Items { get; set; }



        public RemoteExplorerControlVM()
        {

            //TODO
         
        }


    }
}
