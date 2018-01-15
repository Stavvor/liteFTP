
using System.Windows;

namespace liteFTP
{
    public interface IAlertService
    {
        void Show(string message, string title = "");
        bool YesNo(string message, string title="");
    }
}
