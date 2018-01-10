using liteFTP.Models;
using System;
using System.Collections.ObjectModel;
using System.Security;
using System.Windows.Input;
using liteFTP.Helpers;
using System.Threading.Tasks;

namespace liteFTP.ViewModels
{
    public class AuthorizationControlVM : BaseViewModel
    {
        public string ServerNameInput { get; set; }

        public string UserNameInput { get; set; }

        public SecureString PasswordInput { get { return PasswordBoxBindingHelper.Password; } }

        public ObservableCollection<FTPcredentialsModel> AuthorizedCredentials { get; set; }

        public FTPcredentialsModel UnauthorizedCredentials { get; set; }

        public FTPclientModel ClientModel { get; set; }

        public ICommand ConnectCommand { get; set; }


        public AuthorizationControlVM()
        {
            ServerNameInput = "rhdhdfhfdggsd.cba.pl";
            UserNameInput = "test@rhdhdfhfdggsd.cba.pl";

            AuthorizedCredentials = new ObservableCollection<FTPcredentialsModel>();

            ConnectCommand = new RelayCommand(async () => await CreateCredentials());
        }

        private async Task CreateCredentials()
        {
            var readablePass = PasswordInput.ToStandardString();

            if (String.IsNullOrEmpty(ServerNameInput) || String.IsNullOrEmpty(UserNameInput) || String.IsNullOrEmpty(readablePass))
            {
                IoC.Get<IAlertService>().Show("Fill all fields!");
                return;
            }


            UnauthorizedCredentials = new FTPcredentialsModel(ServerNameInput, UserNameInput, PasswordInput);

            if (await Authorize())
                AuthorizedCredentials.Add(UnauthorizedCredentials);
            else
                IoC.Get<IAlertService>().Show("Incorect credentials!");
        }

        private async  Task<bool> Authorize()
        {
            //TODO IoC container
            ClientModel = new FTPclientModel(UnauthorizedCredentials);

            return await ClientModel.AuthorizeFTPConnectionsAsync();
        }
    }
}
