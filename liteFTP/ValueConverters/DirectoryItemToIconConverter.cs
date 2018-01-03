using System;
using System.Globalization;
using liteFTP.Helpers;

namespace liteFTP.ValueConverters
{
    public class DirectoryItemToIconConverter : BaseValueConverter<DirectoryItemToIconConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((DirectoryItems)value)
            {
                case DirectoryItems.Drive:
                    return ResourcesDictionaryQuerry.GetResourceFromDictionary("FontAwesomeDriveIcon");
                case DirectoryItems.File:
                    return ResourcesDictionaryQuerry.GetResourceFromDictionary("FontAwesomeFileIcon");
                default:
                    return ResourcesDictionaryQuerry.GetResourceFromDictionary("FontAwesomeFolderIcon"); //TODO open/closed folder
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
