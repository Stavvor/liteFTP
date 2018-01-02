using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace liteFTP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new DirectoryViewModel();
        }
    }
}
