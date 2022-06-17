using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Scullion
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Model.user33Entities Context
        { get; } = new Model.user33Entities();
        public static Model.User CurrentUser = null;
        public static Model.Product selectedProduct = null;
        public static Model.Order selectedOrder = null;
        public static Model.OrderProduct selectedOrderProduct = null;
        public static List<Model.Product> selectedProducts = null;
    }
}
