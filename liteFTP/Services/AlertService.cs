using System.Windows;

namespace liteFTP
{
    public class AlertService : IAlertService
    {
        public void Show(string message, string title = "")
        {
            MessageBox.Show(message, title);

        }

        public bool YesNo(string message, string title = "")
        {
            var result=MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result==MessageBoxResult.Yes) return true;
            return false;
   
        }
    }
}
