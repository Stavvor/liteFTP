using liteFTP.ViewModels;
using Ninject;


namespace liteFTP
{
    public static class IoC
    {
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        public static MainWindowVM MainVM => Get<MainWindowVM>();

        public static void Setup()
        {
            BindViewModels();
        }

        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }

        private static void BindViewModels()
        {

            Kernel.Bind<MainWindowVM>().ToConstant(new MainWindowVM());

            Kernel.Bind<AuthorizationControlVM>().ToConstant(new AuthorizationControlVM());

            Kernel.Bind<LocalExplorerControlVM>().ToConstant(new LocalExplorerControlVM());

            Kernel.Bind<RemoteExplorerControlVM>().ToConstant(new RemoteExplorerControlVM());

            Kernel.Bind<TransferProgressControlVM>().ToConstant(new TransferProgressControlVM());

            Kernel.Bind<UpDownSpeedControlVM>().ToConstant(new UpDownSpeedControlVM());

            Kernel.Bind<InputBoxVM>().ToConstant(new InputBoxVM());

    }
    }
}
