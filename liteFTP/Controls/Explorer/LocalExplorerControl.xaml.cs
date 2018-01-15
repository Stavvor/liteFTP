using liteFTP.ViewModels;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace liteFTP
{
    /// <summary>
    /// Interaction logic for LocalExplorerControl.xaml
    /// </summary>
    public partial class LocalExplorerControl : UserControl
    {
        public LocalExplorerControl()
        {
            InitializeComponent();
        }

        private void SelectionHandler(object s, RoutedEventArgs a)
        {
            IList items = currentDirList.SelectedItems;
            var collection = items.Cast<DirectoryItemVM>();

            var vm = (LocalExplorerControlVM)DataContext;
            vm.SelectedItems = collection.ToList();
            if(collection.Count()>0)
                vm.CurrentPath = collection.FirstOrDefault().Path;
        }
    }
}
