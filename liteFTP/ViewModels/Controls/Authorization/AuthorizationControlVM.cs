using liteFTP.Models;
using System;
using System.Collections.ObjectModel;
using System.Security;
using System.Windows.Input;
using liteFTP.Helpers;

namespace liteFTP.ViewModels
{
    public class AuthorizationControlVM : BaseViewModel
    {
        public string ServerNameInput { get; set; }

        public string UserNameInput { get; set; }

        public SecureString PasswordInput { get { return PasswordBoxBindingHelper.Password; } }

        public ObservableCollection<FTPcredentialsVM> AuthorizedCredentials { get; set; }

        public FTPcredentialsVM UnauthorizedCredentials { get; set; }

        public FTPclientModel ClientModel { get; set; }

        public ICommand ConnectCommand { get; set; }
        public ICommand ConnectionsHistoryCommand { get; set; } //TODO previous connections list for quick connect feature


        public static AuthorizationControlVM Instance { get; } = new AuthorizationControlVM();

        private AuthorizationControlVM()
        {
            ServerNameInput = "rhdhdfhfdggsd.cba.pl";
            UserNameInput = "test@rhdhdfhfdggsd.cba.pl";

            AuthorizedCredentials = new ObservableCollection<FTPcredentialsVM>();

            ConnectCommand = new RelayCommand(CreateCredentials);
        }

        private void CreateCredentials()
        {
            var readablePass = PasswordInput.ToStandardString();

            if (String.IsNullOrEmpty(ServerNameInput) || String.IsNullOrEmpty(UserNameInput) || String.IsNullOrEmpty(readablePass))
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

            return ClientModel.AuthorizeFTPConnection();
        }
    }
}
