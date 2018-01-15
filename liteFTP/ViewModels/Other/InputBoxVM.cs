using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace liteFTP.ViewModels
{
    public class InputBoxVM : BaseViewModel
    {
        public string Input { get; set; }

        ICommand ConfirmCommand;
        ICommand CancelCommand;

        public InputBoxVM()
        {

        }

        private async void Confirm()
        {

        }

        private async void Cancel()
        {

        }

    }
}
