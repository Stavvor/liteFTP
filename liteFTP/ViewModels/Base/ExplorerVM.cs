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
    public abstract class ExplorerVM : BaseViewModel
    {

        protected string _currentPath;

        protected int _historyIndex;

        protected List<string> _folderHistory;

        protected List<DirectoryItemVM> _selectedItems;

        public virtual string CurrentPath
        {
            get
            {
                return _currentPath;
            }
            set
            {
                _currentPath = value;
            }
        }

        public List<DirectoryItemVM> SelectedItems
        {
            get
            {
                return _selectedItems;
            }
            set
            {
                _selectedItems = value;
                IoC.Get<TransferProgressControlVM>().TransferQueue = new ObservableCollection<DirectoryItemVM>(value);
            }
        }

        public string NewDirectory { get; set; }

        public ObservableCollection<DirectoryItemVM> Items { get; set; }

        public ObservableCollection<DirectoryItemVM> CurrentFolderItems { get; set; }

        public FTPclientModel Ftp { get; set; }
        
        public string FileFolderName { get; set; }

        public ICommand GoToPreviousFolder { get; set; }
        public ICommand GoToNextFolder { get; set; }
        public ICommand GoToParrentFolder { get; set; }

        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand NewDirectoryCommand { get; set; }

        protected abstract void ExpandTree(string dir);

        protected void PrevFolder()
        {
            if (_historyIndex > 0)
            {
                CurrentPath = _folderHistory[_historyIndex - 1];
                _historyIndex--;
            }
        }

        protected void NextFolder()
        {
            if (_historyIndex < _folderHistory.Count - 1)
            {
                CurrentPath = _folderHistory[_historyIndex + 1];
                _historyIndex++;
            }
        }

        protected void ParrentFolder()
        {
            if (!String.IsNullOrEmpty(CurrentPath))
            {
                string[] path = CurrentPath.Split('\\');
                if (path.Length <= 2 && path.Any(i => String.IsNullOrEmpty(i)))
                {
                    CurrentFolderItems.Clear();
                    DirectoryItemVM item = Items.FirstOrDefault(i => i.Name == $"{path.FirstOrDefault()}\\");
                    item.ClearChildren();
                }
                var parrent = Directory.GetParent(CurrentPath);
                if (parrent != null)
                    CurrentPath = parrent.FullName;
            }
        }

        protected void ProcessEnded(object sender, EventArgs e)
        {
            Process p = (Process)sender;
            var name = DirectoryManager.GetNameFromPath(p.StartInfo.FileName);

            Task deleteTask = Task.Run(async () => await Ftp.FtpDeleteFileAsync(name, DirectoryItems.File));
            Task uploadTask = Task.Run(async () => await Ftp.FtpUploadFileAsync(p.StartInfo.FileName));

            File.Delete(p.StartInfo.FileName);
            p.Close();
        }
    }
}
