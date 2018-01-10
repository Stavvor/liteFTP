namespace liteFTP.ViewModels
{

    public class MainWindowVM : BaseViewModel
    {
        public AuthorizationControlVM Authorization { get; }

        public LocalExplorerControlVM LocalExplorer { get; }

        public RemoteExplorerControlVM RemoteExplorer { get; }

        public TransferProgressControlVM TransferProgress { get; set; }

        public UpDownSpeedControlVM UpDownSpeed { get; set; }

        //TODO cards
        public MainWindowVM()
        {
            Authorization = IoC.Get<AuthorizationControlVM>(); //TODO IoC container

            LocalExplorer = IoC.Get<LocalExplorerControlVM>();

            RemoteExplorer = IoC.Get<RemoteExplorerControlVM>(); //TODO 

            UpDownSpeed = IoC.Get<UpDownSpeedControlVM>();

            TransferProgress = IoC.Get<TransferProgressControlVM>();
        }
    }
}
