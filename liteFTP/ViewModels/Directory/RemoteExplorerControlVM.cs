using liteFTP.Controls;
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
    public class RemoteExplorerControlVM : ExplorerVM
    {

        public ICommand DownloadCommand { get; set; }

        public RemoteExplorerControlVM()
        {
            SelectedItems = new List<DirectoryItemVM>();

            DownloadCommand = new RelayCommand(async () => await Download());
            EditCommand = new RelayCommand(async () => await Edit());
            DeleteCommand = new RelayCommand(async () => await Delete());
            NewDirectoryCommand = new RelayCommand(async () => await NewDir());

        }

        public ObservableCollection<DirectoryItemVM> GetItemsFromResponse(List<string> response)
        {
            if (response.Count == 0)
            {
                return null; 
            }

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

        private async Task Download()
        {
            Ftp = new FTPclientModel(IoC.Get<AuthorizationControlVM>().AuthorizedCredentials.FirstOrDefault()); 


            var target = IoC.Get<LocalExplorerControlVM>().CurrentPath;
            foreach (var item in SelectedItems)
            {
                await Ftp.FtpDownloadFilesAsync(item.Path, $"{target}\\{item.Name}");
                IoC.Get<LocalExplorerControlVM>().CurrentFolderItems.Add(item);
                IoC.Get<TransferProgressControlVM>().TransferQueue.Remove(item);
            }
        }

        private async Task Edit()
        {
            Ftp = new FTPclientModel(IoC.Get<AuthorizationControlVM>().AuthorizedCredentials.FirstOrDefault()); 
            CurrentPath = Ftp.Uri;

            var path=Path.GetTempPath();

            var item = SelectedItems.FirstOrDefault();

            if (item != null)
            {
                if (item.Type == DirectoryItems.Folder)
                {
                    IoC.Get<IAlertService>().Show("Cant edit a directory!");
                    return;
                }

                await Ftp.FtpDownloadFilesAsync(item.Path, $"{path}\\{item.Name}");

                Process p = new Process();
                p.Exited += new EventHandler(ProcessEnded);
                p.StartInfo.FileName = $"{path}\\{item.Name}";
                p.EnableRaisingEvents = true;
                p.Start();
            }
            
        }

        private async Task Delete()
        {
            Ftp = new FTPclientModel(IoC.Get<AuthorizationControlVM>().AuthorizedCredentials.FirstOrDefault());

            if(IoC.Get<IAlertService>().YesNo("Are you sure that you want to delete all selected item(s)!"))
            {
                foreach (var item in SelectedItems)
                {
                    await Ftp.FtpDeleteFileAsync(item.Path, item.Type);
                    Items.Remove(item);
                }
            }
        }
        
        private async Task NewDir()
        {
            var inputBox = new InputBox();
            inputBox.Show();


        }

        protected override void ExpandTree(string dir)
        {
            throw new NotImplementedException();
        }

        private async Task CreateDir(InputBoxVM input)
        {
            if (!String.IsNullOrEmpty(FileFolderName))
            {
                Ftp = new FTPclientModel(IoC.Get<AuthorizationControlVM>().AuthorizedCredentials.FirstOrDefault());

                await Ftp.CreateDirectorysAsync(FileFolderName);
                Items.Add(new DirectoryItemVM(CurrentPath, DirectoryItems.Folder));
            }
            
        }
    }
}
