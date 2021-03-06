﻿using liteFTP.Models;
using liteFTP.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace liteFTP
{
    /// <summary>
    /// Interaction logic for RemoteExplorerControl.xaml
    /// </summary>
    public partial class RemoteExplorerControl : UserControl
    {
        public RemoteExplorerControl()
        {
            InitializeComponent();

           
        }

        private void SelectionHandler(object s, RoutedEventArgs a)
        {
            IList items = currentDirList.SelectedItems;
            var collection = items.Cast<DirectoryItemVM>();

            var vm = (RemoteExplorerControlVM)DataContext;
            vm.SelectedItems = collection.ToList();
        }
    }
}
