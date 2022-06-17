using Microsoft.Win32;
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
    /// Логика взаимодействия для AddEditProductPage.xaml
    /// </summary>
    public partial class AddEditProductPage : Page
    {
        public string _imageMain;
        public AddEditProductPage()
        {
            InitializeComponent();
            if (App.selectedProduct != null)
            {
                Title = "Редактирование продукта";
                TBoxID.Text =App.selectedProduct.ProductID.ToString();
                TBoxID.IsReadOnly =true;
                TBoxArticle.Text = App.selectedProduct.ProductArticle;
                TBoxName.Text = App.selectedProduct.ProductName;
                TBoxUnit.Text = App.selectedProduct.ProductUnit;
                TBoxCost.Text = App.selectedProduct.ProductCost.ToString("N2");
                TBoxMaxDiscount.Text = App.selectedProduct.ProductMaxDiscount.ToString();
                CBoxManufacture.SelectedIndex = App.selectedProduct.ProductManufactureID-1;
                CBoxManufacture.ItemsSource = App.Context.Manufacture.ToList() ;
                CBoxProductType.SelectedIndex = App.selectedProduct.ProductTypeID-1;
                CBoxProductType.ItemsSource =App.Context.ProductType.ToList();
                CBoxSupplier.SelectedIndex = App.selectedProduct.ProductSupplierID-1;
                CBoxSupplier.ItemsSource =App.Context.Supplier.ToList();
                TBoxCurrentDiscount.Text = App.selectedProduct.ProductCurrentDiscount.ToString();
                TBoxCountInStock.Text = App.selectedProduct.ProductCountInStock.ToString();
                TBoxDescription.Text = App.selectedProduct.ProductDescription;
                if (App.selectedProduct.ProductImage != null)
                {
                    ImageProduct.Source =new BitmapImage(new Uri(App.selectedProduct.ProductImage,UriKind.RelativeOrAbsolute));
                    _imageMain = App.selectedProduct.ProductImage;

                }
            }
            else
            {
                TextID.Visibility = Visibility.Collapsed;
                TBoxID.Visibility = Visibility.Collapsed;
                BtnDelete.Visibility = Visibility.Collapsed;
                CBoxManufacture.ItemsSource = App.Context.Manufacture.ToList();
                CBoxProductType.ItemsSource = App.Context.ProductType.ToList();
                CBoxSupplier.ItemsSource = App.Context.Supplier.ToList();
            }
        }

        private string CheckError()
        {
            var errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TBoxName.Text))
                errorBuilder.AppendLine("Название обязательно для заполнения;");

            var productForDB = App.Context.Product.FirstOrDefault(p => p.ProductName.ToLower() == TBoxName.Text.ToLower() 
            && p.ProductArticle.ToLower() == TBoxArticle.Text.ToLower());
            if (productForDB != null && productForDB != App.selectedProduct)
                errorBuilder.AppendLine("Такой продукт или артикул уже есть в базе;");


            if (string.IsNullOrWhiteSpace(TBoxUnit.Text))
                errorBuilder.AppendLine("Ед. измерения обязательно для заполенения;");

            decimal cost = 0;
            if (decimal.TryParse(TBoxCost.Text, out cost) == false || cost < 0)
                errorBuilder.AppendLine("Стоимость должен быть положительным числом;");

            int maxDiscount = 0;
            if (int.TryParse(TBoxMaxDiscount.Text, out maxDiscount) == false || maxDiscount < 0 || maxDiscount > 100)
                errorBuilder.AppendLine("Размер скидки должен быть положительным числом в диапазоне от 0 до 100%;");


            if (string.IsNullOrWhiteSpace(CBoxManufacture.Text))
                errorBuilder.AppendLine("Производитель обязательно для заполенения;");

            if (string.IsNullOrWhiteSpace(CBoxProductType.Text))
                errorBuilder.AppendLine("Тип продукта обязательно для заполенения;");

            if (string.IsNullOrWhiteSpace(CBoxSupplier.Text))
                errorBuilder.AppendLine("Поставщик обязательно для заполенения;");

            int currentDiscount = 0;
            if (int.TryParse(TBoxCurrentDiscount.Text, out currentDiscount) == false || currentDiscount < 0 || currentDiscount > 100)
                errorBuilder.AppendLine("Размер скидки должен быть положительным числом в диапазоне от 0 до 100%;");


            int countInStock = 0;
            if (int.TryParse(TBoxCountInStock.Text, out countInStock) == false || countInStock < 0)
                errorBuilder.AppendLine("Кол-во на складе должен быть положительным числом;");

            if (errorBuilder.Length > 0)
                errorBuilder.Insert(0, "Устраните следующие ошибки:\n");

            return errorBuilder.ToString();
        }
        private void BtnSelectedImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                _imageMain = "/Images/Товар/" + ofd.SafeFileName;
                ImageProduct.Source = new BitmapImage(new Uri(_imageMain, UriKind.RelativeOrAbsolute));

            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var errorMessage = CheckError();
                if (errorMessage.Length > 0)
                {
                    MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (App.selectedProduct == null)
                    {

                        var product = new Model.Product
                        {
                            ProductArticle= TBoxArticle.Text,
                            ProductName = TBoxName.Text,
                            ProductUnit = TBoxUnit.Text,
                            ProductCost = decimal.Parse(TBoxCost.Text),
                            ProductMaxDiscount = int.Parse(TBoxMaxDiscount.Text),
                            ProductManufactureID = int.Parse(CBoxManufacture.SelectedValue.ToString()),
                            ProductTypeID = int.Parse(CBoxProductType.SelectedValue.ToString()),
                            ProductSupplierID = int.Parse(CBoxSupplier.SelectedValue.ToString()),
                            ProductCurrentDiscount = int.Parse(TBoxCurrentDiscount.Text),
                            ProductCountInStock = int.Parse(TBoxCountInStock.Text),
                            ProductDescription = TBoxDescription.Text,
                            ProductImage = _imageMain,
                        };
                        App.Context.Product.Add(product);
                        App.Context.SaveChanges();
                        MessageBox.Show($"Продукт {product.ProductName} добавлен", "Внимание",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else
                    {
                        App.selectedProduct.ProductArticle = TBoxArticle.Text;
                        App.selectedProduct.ProductName = TBoxName.Text;
                        App.selectedProduct.ProductUnit = TBoxUnit.Text;
                        App.selectedProduct.ProductMaxDiscount = int.Parse(TBoxMaxDiscount.Text);
                        App.selectedProduct.ProductCost = decimal.Parse(TBoxCost.Text);
                        App.selectedProduct.ProductManufactureID = int.Parse(CBoxManufacture.SelectedValue.ToString());
                        App.selectedProduct.ProductTypeID = int.Parse(CBoxProductType.SelectedValue.ToString());
                        App.selectedProduct.ProductSupplierID = int.Parse(CBoxSupplier.SelectedValue.ToString());
                        App.selectedProduct.ProductCurrentDiscount = int.Parse(TBoxCurrentDiscount.Text);
                        App.selectedProduct.ProductCountInStock = int.Parse(TBoxCountInStock.Text);
                        App.selectedProduct.ProductDescription = TBoxDescription.Text;
                        App.selectedProduct.ProductImage = _imageMain;
                        App.Context.SaveChanges();
                        MessageBox.Show($"Продукт {App.selectedProduct.ProductName} изменен", "Внимание",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    NavigationService.GoBack();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int countOrder = 0;
                if (MessageBox.Show($"Вы уверены, что хотите удалить продукт {App.selectedProduct.ProductName}"
                    , "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    foreach (Model.OrderProduct orderProduct in App.Context.OrderProduct.Cast<Model.OrderProduct>())
                    {
                        if (orderProduct.ProductID == App.selectedProduct.ProductID)
                        {
                            countOrder++;
                            MessageBox.Show($"Удалить нельзя продукт {App.selectedProduct.ProductName} " +
                                $"содержит заказ №{orderProduct.OrderID}", "Внимание",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    if (countOrder == 0)
                    {
                        App.Context.Product.Remove(App.selectedProduct);
                        App.Context.SaveChanges();
                        MessageBox.Show($"Продукт {App.selectedProduct.ProductName} удален", "Внимание",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService.GoBack();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
