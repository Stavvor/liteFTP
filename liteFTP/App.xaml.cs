using System.Windows;

namespace liteFTP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IoCSetup();


        }

        private void IoCSetup()
        {
            IoC.Setup();

            IoC.Kernel.Bind<IAlertService>().ToConstant(new AlertService());
        }
    }
}
