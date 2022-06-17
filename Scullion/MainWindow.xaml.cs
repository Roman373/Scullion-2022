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

namespace Scullion
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FrameMain.Navigate(new View.Pages.LoginPage());
        }

        private void FrameMain_Navigated(object sender, NavigationEventArgs e)
        {
            if (App.CurrentUser != null)
            {
                TextFIO.Text = $"{App.CurrentUser.UserSurname} {App.CurrentUser.UserName} {App.CurrentUser.UserPatronymic}";
            }
            Page page = e.Content as Page;

            if (page.Title == "Авторизация")
            {
                TextFIO.Visibility = Visibility.Collapsed;
                BtnBack.Visibility = Visibility.Collapsed;
            }
            else
            {
                TextFIO.Visibility = Visibility.Visible;
                BtnBack.Visibility = Visibility.Visible;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (FrameMain.CanGoBack)
            {
                App.selectedProduct = null;
                App.selectedOrder = null;
                FrameMain.GoBack();
            }
        }
    }
}
