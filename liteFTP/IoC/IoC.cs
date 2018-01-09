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

        private static void BindViewModels()
        {

            Kernel.Bind<MainWindowVM>().ToConstant(new MainWindowVM());

        }

        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
