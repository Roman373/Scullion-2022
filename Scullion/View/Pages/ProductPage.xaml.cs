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
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public List<Model.Product> selectedProduct=null;
        public ProductPage()
        {
            InitializeComponent();
            CBoxSortByCost.SelectedIndex = 0;
            CBoxDiscount.SelectedIndex = 0;
            if (App.CurrentUser.UserRole == 1)
            {
                BtnAddProduct.Visibility = Visibility.Visible;
            }
            else if (App.CurrentUser.UserRole == 3)
            {
                BtnAddProduct.Visibility = Visibility.Collapsed;
            }
            else
            {
                BtnAddProduct.Visibility = Visibility.Collapsed;
            } 
            UpdateProduct();
        }
        private void UpdateProduct()
        {
            var product = App.Context.Product.ToList();

            if (CBoxSortByCost.SelectedIndex == 0)
                product = product.OrderBy(p => p.ProductCost).ToList();
            else
                product = product.OrderByDescending(p => p.ProductCost).ToList();

            if (CBoxDiscount.SelectedIndex == 1)
                product = product.Where(p => p.ProductCurrentDiscount >= 0 && p.ProductCurrentDiscount < 10).ToList();

            if (CBoxDiscount.SelectedIndex ==2)
                product = product.Where(p => p.ProductCurrentDiscount >= 10 && p.ProductCurrentDiscount < 15).ToList();
            if (CBoxDiscount.SelectedIndex == 3)
                product = product.Where(p => p.ProductCurrentDiscount >= 15 && p.ProductCurrentDiscount <= 100).ToList();

            product = product.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower()) ||
            p.ProductDescription.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            if (product.Count > 0)
            {
                TextCount.Text = product.Count.ToString();
                TextCountDB.Text = App.Context.Product.Count().ToString();
                StackCount.Visibility = Visibility.Visible;
                TextNullProduct.Visibility = Visibility.Collapsed;
            }
            else
            {
                StackCount.Visibility = Visibility.Collapsed;
                TextNullProduct.Visibility = Visibility.Visible;
                TextNullProduct.Text = "Продукт не найден!!!";

            }
            LBoxProduct.ItemsSource = product;

        }
        private void CBoxSortByCost_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void CBoxDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProduct();
        }
        public int countSelectedProduct = 0;
        private void LBoxProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectedProduct != null)
            {
                countSelectedProduct = selectedProduct.Count;
                if (countSelectedProduct > 0)
                    BtnAddOrder.Visibility = Visibility.Visible;
                else
                    BtnAddOrder.Visibility = Visibility.Collapsed;
                UpdateProduct();
            }
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            App.selectedProduct = null;
            NavigationService.Navigate(new AddEditProductPage());
            UpdateProduct();
        }

        private void BtnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            App.selectedProducts = selectedProduct;
            Windows.LookOrderWindow lookOrderWindow = new Windows.LookOrderWindow();
            lookOrderWindow.ShowDialog();
            UpdateProduct();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            selectedProduct = LBoxProduct.SelectedItems.Cast<Model.Product>().ToList();
            UpdateProduct();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateProduct();
        }
        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new OrderPage());
        }
        private void BtnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            App.selectedProduct = (sender as Button).DataContext as Model.Product;
            NavigationService.Navigate(new AddEditProductPage());
        }
    }
}
