using System;

namespace liteFTP.Models
{
    public class DirectoryItemModel
    {
       
        public DirectoryItems Type { get; set; }

        public string Path { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifyDate { get; set; }

        public string Name { get; set; }


        public DirectoryItemModel(DirectoryItems type, string path, string name)
        {
            Type = type;
            Path = path;
            Name = name;
        }

        public DirectoryItemModel(DirectoryItems type, string path, DateTime creationDate, DateTime modifyDate, string name)
        {
            Type = type;
            Path = path;
            CreationDate = creationDate;
            LastModifyDate = modifyDate;
            Name = name;
        }

        //TODO override tostring
    }
}
