
using liteFTP.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace liteFTP
{
  
    public class MonitorItemSelectedTreeViewItemChangeAttachedProperty : BaseAttachedProperty<MonitorItemSelectedTreeViewItemChangeAttachedProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
         
            var treeView = sender as TreeView;

            if (treeView == null)
                return;

            treeView.SelectedItemChanged -= TreeViewItemSelectedChanged;

            if ((bool)e.NewValue)
                treeView.SelectedItemChanged += TreeViewItemSelectedChanged;

        }

     
        private void TreeViewItemSelectedChanged(object sender, RoutedEventArgs e)
        {
        
            var treeView = sender as TreeView;

            if (treeView == null)
                return;

            var SelectedTreeViewItem = treeView.SelectedItem as DirectoryItemVM;

            IoC.Get<LocalExplorerControlVM>().SelectedItem = SelectedTreeViewItem;
        }


    }
}