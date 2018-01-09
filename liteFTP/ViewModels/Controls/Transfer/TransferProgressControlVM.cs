using System.Collections.ObjectModel;


namespace liteFTP.ViewModels
{
    public class TransferProgressControlVM : BaseViewModel
    {
        private double _progressValue;

        public double ProgressValue {
            get
            {
                return _progressValue;
            }
            set
            {
                _progressValue = value;
            }
        }

        public ObservableCollection<DirectoryItemVM> TransferQueue { get; set; }

        public static TransferProgressControlVM Instance { get; } = new TransferProgressControlVM();

        private TransferProgressControlVM()
        {

        }
    }
}
