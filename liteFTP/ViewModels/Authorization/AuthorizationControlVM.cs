using liteFTP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace liteFTP.ViewModels
{
    public class AuthorizationControlVM : BaseViewModel
    {
        public string ServerNameInput { get; set; }

        public string UserNameInput { get; set; }

        public string PasswordInput { get; set; }

        public ObservableCollection<FTPcredentialsVM> AuthorizedCredentials { get; set; }

        public FTPcredentialsVM UnauthorizedCredentials { get; set; }

        public FTPclientModel ClientModel { get; set; }

        public ICommand ConnectCommand { get; set; }
        public ICommand ConnectionsHistoryCommand { get; set; } //TODO previous connections list for quick connect feature

        public AuthorizationControlVM()
        {
            ServerNameInput = "rhdhdfhfdggsd.cba.pl";
            UserNameInput = "test@rhdhdfhfdggsd.cba.pl";
            PasswordInput = "Test123";

            AuthorizedCredentials = new ObservableCollection<FTPcredentialsVM>();

            ConnectCommand = new RelayCommand(CreateCredentials);
        }

        private void CreateCredentials()
        {
            if (String.IsNullOrEmpty(ServerNameInput) || String.IsNullOrEmpty(UserNameInput) || String.IsNullOrEmpty(PasswordInput))
                return;
                //TODO messagebox

            UnauthorizedCredentials = new FTPcredentialsVM(ServerNameInput, UserNameInput, PasswordInput);

            if (Authorize())
                AuthorizedCredentials.Add(UnauthorizedCredentials);
        }

        private bool Authorize()
        {
            //TODO IoC container
            ClientModel = new FTPclientModel(UnauthorizedCredentials);

            //ClientModel.FtpDownloadFile("test.txt", "C:\\Users\\Stavor\\Documents\\Visual Studio 2017\\Projects\\liteFTP\\liteFTP\\bin\\Debug\\test.txt");

            return ClientModel.AuthorizeFTPConnection();
        }
    }
}
