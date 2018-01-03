using System;

namespace liteFTP.Models
{
    public class DirectoryItemModel
    {
       
        public DirectoryItems Type { get; set; }

        public string Path { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifyDate { get; set; }

        public string Name { get { return this.Type == DirectoryItems.Drive ? this.Path : DirectoryManager.GetNameFromPath(Path); }  }


        public DirectoryItemModel(DirectoryItems type, string path)
        {
            Type = type;
            Path = path;
        }

        public DirectoryItemModel(DirectoryItems type, string path, DateTime creationDate, DateTime modifyDate)
        {
            Type = type;
            Path = path;
            CreationDate = creationDate;
            LastModifyDate = modifyDate;
        }

        //TODO override tostring
    }
}
