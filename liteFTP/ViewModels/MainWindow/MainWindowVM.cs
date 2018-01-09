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
            Authorization = AuthorizationControlVM.Instance; //TODO IoC container

            LocalExplorer= LocalExplorerControlVM.Instance;

            RemoteExplorer = new RemoteExplorerControlVM(); //TODO 

            UpDownSpeed = UpDownSpeedControlVM.Instance;

            TransferProgress = TransferProgressControlVM.Instance;
        }
    }
}
