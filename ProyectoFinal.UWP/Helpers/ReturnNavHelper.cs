using ProyectoFinal.UWP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ProyectoFinal.UWP.Helpers
{
    public class ReturnNavHelper
    {
        public static bool TryGoBack()
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
                return true;
            }
            return false;
        }
    }
}
