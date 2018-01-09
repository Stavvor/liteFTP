using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liteFTP.ViewModels
{
    public class TransferProgressControlVM : BaseViewModel
    {
        public double ProgressValue { get; set; }

        public static TransferProgressControlVM Instance { get; } = new TransferProgressControlVM();

        private TransferProgressControlVM()
        {

        }
    }
}
