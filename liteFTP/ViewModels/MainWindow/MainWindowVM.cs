using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liteFTP.ViewModels
{

    public class MainWindowVM
    {
        public AuthorizationControlVM Authorization { get; }

        public DirectoryVM Directory { get; }

        //TODO cards
        public MainWindowVM()
        {
            Authorization = new AuthorizationControlVM();
            Directory = new DirectoryVM();        
        }
    }
}
