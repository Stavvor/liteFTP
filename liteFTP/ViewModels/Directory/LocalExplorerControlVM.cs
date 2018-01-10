using liteFTP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace liteFTP.ViewModels
{
    public class LocalExplorerControlVM : BaseViewModel
    {
        private string _currentPath = ""; //TODO

        private DirectoryItemVM _selectedItem;

        private string _comboText;

        private List<string> _folderHistory;

        private int _historyIndex;

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

                if (!_folderHistory.Contains(value) && Directory.Exists(value))
                {
                    _folderHistory.Add(value);
                    _historyIndex++;
                }
                ComboText = value;
                ExpandTree(value);

                List<string> myItems = new List<string>();

                foreach (var item in CurrentFolderItems)
                {
                    myItems.Add(item.Path);
                }
                
                ComboItems = CollectionViewSource.GetDefaultView(myItems.ToArray());
            }
        }

        public ObservableCollection<DirectoryItemVM> Items { get; set; }

        public ObservableCollection<DirectoryItemVM> CurrentFolderItems { get; set; }

        public ICollectionView ComboItems { get; set; }

        public string ComboText
        {
            get
            {
                return _comboText;
            }
            set
            {
                if(ComboItems!=null)
                    ComboItems.Filter = item => item.ToString().ToLower().Contains(value.ToLower());
                _comboText = value;
                CurrentPath = value;
            }
        }

        public DirectoryItemVM SelectedItem {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                CurrentPath = value.Path;
                ComboText = value.Path;
            }
        }

        public static LocalExplorerControlVM Instance { get ;} = new LocalExplorerControlVM();

        public FTPclientModel Ftp { get; set; }

        public ICommand GoToPreviousFolder { get; set; }
        public ICommand GoToNextFolder { get; set; }
        public ICommand GoToParrentFolder { get; set; }

        public ICommand UploadCommand { get; set; }

        private LocalExplorerControlVM()
        { 
            var children = DirectoryManager.GetDrives();

            Items = new ObservableCollection<DirectoryItemVM>(
                children.Select(drive => new DirectoryItemVM(drive.Path, DirectoryItems.Drive)
                ));

            _folderHistory = new List<string>();
            _historyIndex = 0;

            GoToPreviousFolder = new RelayCommand(PrevFolder);
            GoToNextFolder = new RelayCommand(NextFolder);
            GoToParrentFolder = new RelayCommand(ParrentFolder);

            UploadCommand = new RelayCommand(async () => await UploadFile());

            
        }

        private void ExpandTree(string dir)
        {

            if (String.IsNullOrEmpty(CurrentPath) || !Directory.Exists(CurrentPath))
                return;

            string[] path=CurrentPath.Split('\\');
            if (path.Length == 0)
                return;
            string fullPath=null;

            DirectoryItemVM currentPlace =null;  

            foreach (var step in path)
            {
                if (currentPlace == null && !String.IsNullOrEmpty(step))
                {
                    var drive = $"{step}\\";
                    currentPlace = Items.Where(i => i.Name == drive).SingleOrDefault();
                }
                    
                else if(!String.IsNullOrEmpty(step))
                    currentPlace = currentPlace.Children.Where(i => i.Name == step).ToList().SingleOrDefault();

                currentPlace.ExpandDirectory();
            }
            CurrentPath = dir;
        }

        private void PrevFolder()
        {
            if (_historyIndex > 0)
            {
                CurrentPath = _folderHistory[_historyIndex - 1];
                _historyIndex--;
            }
        }

        private void NextFolder()
        {
            if (_historyIndex < _folderHistory.Count-1)
            {
                CurrentPath = _folderHistory[_historyIndex + 1];
                _historyIndex++;
            }
        }

        private void ParrentFolder()
        {
            if (!String.IsNullOrEmpty(CurrentPath))
            {
                string[] path = CurrentPath.Split('\\');
                if (path.Length<=2 && path.Any(i=>String.IsNullOrEmpty(i)))
                {
                    CurrentFolderItems.Clear();
                    DirectoryItemVM item = Items.FirstOrDefault(i => i.Name == $"{path.FirstOrDefault()}\\");
                    item.ClearChildren();
                }
                var parrent = Directory.GetParent(CurrentPath);
                if(parrent!=null)
                    CurrentPath = parrent.FullName;
            }
        }

        private async Task UploadFile()
        {
            Ftp = new FTPclientModel(AuthorizationControlVM.Instance.AuthorizedCredentials.FirstOrDefault()); //TODO IoC container

            await Ftp.FtpUploadFileAsync(CurrentPath);
        }
    }
}
