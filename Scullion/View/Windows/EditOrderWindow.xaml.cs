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
using System.Windows.Shapes;

namespace Scullion.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditOrderWindow.xaml
    /// </summary>
    public partial class EditOrderWindow : Window
    {
        public EditOrderWindow()
        {
            InitializeComponent();
            if (App.selectedOrderProduct != null)
            {
                Title = "Редактирование заказа";
                DateDelivery.SelectedDate = App.selectedOrderProduct.Order.OrderDateDelivery;
                CBoxStatus.SelectedIndex = App.selectedOrderProduct.Order.OrderStatusID - 1;
                CBoxStatus.ItemsSource = App.Context.Status.ToList();
            }
        }

        private void BtnSaveOrder_Click(object sender, RoutedEventArgs e)
        {

            if (App.selectedOrderProduct != null)
            {
                App.selectedOrderProduct.Order.OrderDateDelivery = (DateTime)DateDelivery.SelectedDate;
                App.selectedOrderProduct.Order.OrderStatusID = int.Parse(CBoxStatus.SelectedValue.ToString());
                App.Context.SaveChanges();
                MessageBox.Show($"Заказ №{App.selectedOrderProduct.Order.OrderID} изменен",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }
    }
}
