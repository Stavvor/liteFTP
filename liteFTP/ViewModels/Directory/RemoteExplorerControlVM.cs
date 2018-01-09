using liteFTP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace liteFTP.ViewModels
{
    public class RemoteExplorerControlVM : BaseViewModel
    {
        private string _currentPath;

        public string CurrentPath {
            get {
                return _currentPath;
            }
            set
            {
                _currentPath = value;
            }
        }
        public ObservableCollection<DirectoryItemVM> Items { get; set; }

        public List<DirectoryItemVM> SelectedItems { get; set; }

        public FTPclientModel Ftp { get; set; }

        public ICommand SyncCommand { get; set; }
        public ICommand DownloadCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public RemoteExplorerControlVM()
        {
            SelectedItems = new List<DirectoryItemVM>();

            SyncCommand = new RelayCommand(async () => await Sync());
            DownloadCommand = new RelayCommand(async () => await Download());
            EditCommand = new RelayCommand(async () => await Edit());
            DeleteCommand = new RelayCommand(async () => await Delete());

            //TODO

        }

        private ObservableCollection<DirectoryItemVM> GetItemsFromResponse(List<string> response)
        {
            if (response.Count == 0)
                return null; //TODO messagebox 

            Items = new ObservableCollection<DirectoryItemVM>();

            foreach (var item in response)
            {
                string fileName = item.Split(' ').Last(); //TODO error check
                DirectoryItems type;
                if (item[0] == 'd')
                    type = DirectoryItems.Folder;
                else
                    type = DirectoryItems.File;

                Items.Add(new DirectoryItemVM(fileName, type));
            }

            return Items;
        }

        private async Task Sync()
        {
            Ftp = new FTPclientModel(AuthorizationControlVM.Instance.AuthorizedCredentials.FirstOrDefault()); //TODO IoC container
            CurrentPath = Ftp.Uri;
            List<string> response= await Ftp.FtpGetAllFiles();
            GetItemsFromResponse(response);
        }

        private async Task Download()
        {
            Ftp = new FTPclientModel(AuthorizationControlVM.Instance.AuthorizedCredentials.FirstOrDefault()); //TODO IoC container
            CurrentPath = Ftp.Uri;

            var target = LocalExplorerControlVM.Instance.CurrentPath;
            foreach (var item in SelectedItems)
            {
                await Ftp.FtpDownloadFile(item.Path, $"{target}\\{item.Name}");
                LocalExplorerControlVM.Instance.CurrentFolderItems.Add(item);
            }
        }

        private async Task Edit()
        {
            Ftp = new FTPclientModel(AuthorizationControlVM.Instance.AuthorizedCredentials.FirstOrDefault()); //TODO IoC container
            CurrentPath = Ftp.Uri;

            var path=Path.GetTempPath();

            var item = SelectedItems.FirstOrDefault();

            await Ftp.FtpDownloadFile(item.Path, $"{path}\\{item.Name}");

            Process p = new Process();
            p.Exited += new EventHandler(ProcessEnded);
            p.StartInfo.FileName = $"{path}\\{item.Name}";
            p.EnableRaisingEvents = true;
            p.Start();
        }

        private async Task Delete()
        {
            Ftp = new FTPclientModel(AuthorizationControlVM.Instance.AuthorizedCredentials.FirstOrDefault()); //TODO IoC container
            CurrentPath = Ftp.Uri;

            foreach (var item in SelectedItems)
            {
                await Ftp.FtpDeleteFile(item.Path);
                Items.Remove(item);
            }

        }

        private void ProcessEnded(object sender, EventArgs e)
        {
            Process p = (Process)sender;
            var name=DirectoryManager.GetNameFromPath(p.StartInfo.FileName);

            Task deleteTask = Task.Run(async () => await Ftp.FtpDeleteFile(name));
            Task uploadTask = Task.Run(async () => await Ftp.FtpUploadFile(p.StartInfo.FileName));

            File.Delete(p.StartInfo.FileName);
            p.Close();
        }
    }
}
