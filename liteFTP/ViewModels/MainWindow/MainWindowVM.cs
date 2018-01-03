namespace liteFTP.ViewModels
{

    public class MainWindowVM : BaseViewModel
    {
        public AuthorizationControlVM Authorization { get; }

        public LocalExplorerControlVM LocalExplorer { get; }
        public RemoteExplorerControlVM RemoteExplorer { get; }

        //TODO cards
        public MainWindowVM()
        {
            Authorization = new AuthorizationControlVM();

            LocalExplorer = new LocalExplorerControlVM();
            RemoteExplorer = new RemoteExplorerControlVM(); 
        }
    }
}
