using System;
using  System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liteFTP.Helpers
{
    public static class ResourcesDictionaryQuerry
    {
        public static object GetResourceFromDictionary(string Key)
        {
            object resource = null;

            try
            {
                resource = Application.Current.FindResource(Key);
            }
            catch (Exception ex)
            {

            }

            return resource;
        }
    }
}
