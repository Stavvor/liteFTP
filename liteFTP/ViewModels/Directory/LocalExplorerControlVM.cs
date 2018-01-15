using liteFTP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace liteFTP.ViewModels
{
    public class LocalExplorerControlVM : ExplorerVM
    {
        private string _comboText;

        public override string CurrentPath {
            get {
                return _currentPath;
            }
            set {

                _currentPath = value;

                var allItems = DirectoryManager.GetAllItems(value);
                if (allItems.Count() == 0) return;
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

        public ICommand UploadCommand { get; set; }


        public LocalExplorerControlVM()
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
            EditCommand = new RelayCommand(EditFile);
            DeleteCommand = new RelayCommand(DeleteFile);

        }

        protected override void ExpandTree(string dir)
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

                if(currentPlace!=null)
                    currentPlace.ExpandDirectory(); //TODO
            }
            CurrentPath = dir;
        }

        private async Task UploadFile()
        {
            var credentials = IoC.Get<AuthorizationControlVM>().AuthorizedCredentials.FirstOrDefault();
            if (SelectedItems != null && credentials!=null)
            {
                Ftp = new FTPclientModel(credentials); //TODO IoC container

                foreach (var item in SelectedItems)
                {
                    await Ftp.FtpUploadFileAsync(item.Path);
                    IoC.Get<RemoteExplorerControlVM>().Items.Add(item);
                    IoC.Get<TransferProgressControlVM>().TransferQueue.Remove(item);
                }
            }        
        }

        private void DeleteFile()
        {
            if (SelectedItems != null)
            {
                var item = SelectedItems.FirstOrDefault();

                if (item != null && File.Exists(item.Path))
                {
                    File.Delete(item.Path);
                    CurrentFolderItems.Remove(item);
                }
            }
        }

        private void EditFile()
        {
            if (SelectedItems != null)
            {
                var item = SelectedItems.FirstOrDefault();

                if (item != null)
                {
                    if (item.Type == DirectoryItems.Folder)
                    {
                        IoC.Get<IAlertService>().Show("Cant edit a directory!");
                        return;
                    }

                    Process p = new Process();
                    p.StartInfo.FileName = item.Path;
                    p.Start();
                }
            }
        }
    }
}
