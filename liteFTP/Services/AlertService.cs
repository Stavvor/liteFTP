using System.Windows;

namespace liteFTP
{
    public class AlertService : IAlertService
    {
        public void Show(string message, string title = "")
        {
            MessageBox.Show(message, title);
        }
    }
}
