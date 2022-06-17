using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Scullion.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        public OrderPage()
        {
            InitializeComponent();
            CBoxSortBy.SelectedIndex = 0;
            CBoxDiscount.SelectedIndex = 0;
            
                
            UpdateOrder();
        }
        private void UpdateOrder()
        {
            var order = App.Context.OrderProduct.ToList();
            if (CBoxSortBy.SelectedIndex == 0)
                order = order.OrderBy(p => p.TotalCost).ToList();
            else
                order = order.OrderByDescending(p => p.TotalCost).ToList();

            if (CBoxDiscount.SelectedIndex == 1)
                order = order.Where(p => p.TotalDiscount >= 0 && p.TotalDiscount < 10).ToList();

            if (CBoxDiscount.SelectedIndex == 2)
                order = order.Where(p => p.TotalDiscount >= 10 && p.TotalDiscount < 15).ToList();
            if (CBoxDiscount.SelectedIndex == 3)
                order = order.Where(p => p.TotalDiscount >= 15 && p.TotalDiscount <= 100).ToList();


            LBoxOrder.ItemsSource = order;
        }
        private void CBoxSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateOrder();
        }

        private void CBoxDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateOrder();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateOrder();
        }

        private void LBoxOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (App.CurrentUser.UserRole == 3 || App.CurrentUser.UserRole== 1)
            {
                App.selectedOrderProduct = (Model.OrderProduct)LBoxOrder.SelectedItem;
                Windows.EditOrderWindow editOrderWindow = new Windows.EditOrderWindow();
                editOrderWindow.ShowDialog();
                UpdateOrder();
            }
                
        }
    }
}
