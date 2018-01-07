namespace liteFTP.ViewModels
{

    public class MainWindowVM : BaseViewModel
    {
        public AuthorizationControlVM Authorization { get; }

        public LocalExplorerControlVM LocalExplorer { get; }
        public RemoteExplorerControlVM RemoteExplorer { get; }

        public UpDownSpeedControlVM UpDownSpeed { get; }

        //TODO cards
        public MainWindowVM()
        {
            Authorization = AuthorizationControlVM.Instance;

            LocalExplorer= LocalExplorerControlVM.Instance;

            RemoteExplorer = new RemoteExplorerControlVM();

            UpDownSpeed = new UpDownSpeedControlVM();
        }
    }
}
