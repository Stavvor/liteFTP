using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liteFTP.ViewModels
{
    public class UpDownSpeedControlVM
    {
        public uint DownloadSpeed { get; set; }
        public uint UploadSpeed { get; set; }

        public UpDownSpeedControlVM()
        {
            DownloadSpeed = 0;
            UploadSpeed = 0;

            //TODO session dowload history
        }
    }
}
