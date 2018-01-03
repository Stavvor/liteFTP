using System.Security;
using System.Windows.Controls;
using liteFTP.ViewModels;
using liteFTP.Interfaces;

namespace liteFTP 
{
    /// <summary>
    /// Interaction logic for AuthorizationControl.xaml
    /// </summary>
    public partial class AuthorizationControl : UserControl , IPassword
    {
        public AuthorizationControl()
        {
            InitializeComponent();
        }

        public SecureString Password
        {
            get
            {
                return PasswordBox.SecurePassword;
            }
        }
    }
}
