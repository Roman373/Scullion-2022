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
    /// Логика взаимодействия для LookOrderWindow.xaml
    /// </summary>
    public partial class LookOrderWindow : Window
    {
        public double cost = 0, discount = 0, costDiscount = 0;


        public LookOrderWindow()
        {
            InitializeComponent();
            try
            {
                
                OrderUpdate();
                DataProduct.ItemsSource = App.selectedProducts;
                
                CBoxStatus.SelectedIndex = 2;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void OrderUpdate()
        {
            if (App.selectedProducts != null)
            {
                foreach (Model.Product product in App.selectedProducts)
                {
                    cost += (double)product.ProductCost * (100 - product.ProductCurrentDiscount) / 100;
                    costDiscount += (double)product.ProductCost;
                    discount += ((double)product.ProductCost * product.ProductCurrentDiscount) / 100;
                }
                TextTotalCost.Text = cost.ToString("N2");
                if (App.CurrentUser != null)
                {
                    TextFIO.Text = $"{App.CurrentUser.UserSurname} {App.CurrentUser.UserName} {App.CurrentUser.UserPatronymic}";

                }

                TextTotalDiscount.Text = ((discount * 100) / costDiscount).ToString("N2");
                CBoxStatus.ItemsSource = App.Context.Status.ToList();
                CBoxPointDelviery.ItemsSource = App.Context.PointDelivery.ToList();
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OrderUpdate();
        }

        private void BtnSaveOrder_Click(object sender, RoutedEventArgs e)
        {
            if (DateOrder == null || DateDelivery == null || CBoxPointDelviery.SelectedItem == null
                || App.CurrentUser == null || CBoxStatus.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Random random = new Random();
                if (App.selectedOrder == null)
                {
                    var order = new Model.Order
                    {
                        OrderDate = DateOrder.DisplayDate,
                        OrderDateDelivery = DateDelivery.DisplayDate,
                        OrderPointDeliveryID = int.Parse(CBoxPointDelviery.SelectedValue.ToString()),
                        OrderSurname = App.CurrentUser.UserSurname,
                        OrderName = App.CurrentUser.UserName,
                        OrderPatronymic = App.CurrentUser.UserPatronymic,
                        OrderCode = random.Next(999),
                        OrderStatusID = int.Parse(CBoxStatus.SelectedValue.ToString()),
                        OrderUserID = App.CurrentUser.ID,
                    };
                    foreach (Model.Product product in App.selectedProducts)
                    {

                        var orderProduct = new Model.OrderProduct
                        {
                            OrderID = order.OrderID,
                            ProductID = product.ProductID,
                            ProductCount = 1,
                            TotalCost = decimal.Parse(TextTotalCost.Text),
                            TotalDiscount = double.Parse(TextTotalDiscount.Text),

                        };
                        App.Context.OrderProduct.Add(orderProduct);
                    }

                    App.Context.Order.Add(order);
                    App.Context.SaveChanges();
                    MessageBox.Show($"Заказ №{order.OrderID} добавлен", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else { }
            }

        }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedProductOrder = DataProduct.SelectedItem;
            if (selectedProductOrder != null)
            {
                try
                {
                    App.selectedProducts.Remove((Model.Product)selectedProductOrder);

                    App.Context.SaveChanges();
                    OrderUpdate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
