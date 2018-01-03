using System.Security;

namespace liteFTP.Interfaces
{
    interface IPassword
    {
        SecureString Password { get; }
    }
}
